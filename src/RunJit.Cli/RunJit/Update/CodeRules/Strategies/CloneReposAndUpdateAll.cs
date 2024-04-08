using System.Collections.Immutable;
using System.IO.Compression;
using System.Net.Http.Headers;
using AspNetCore.Simple.Sdk.Mediator;
using Extensions.Pack;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Api.Client;
using RunJit.Cli.Auth0;
using RunJit.Cli.AwsCodeCommit;
using RunJit.Cli.ErrorHandling;
using RunJit.Cli.Git;
using RunJit.Cli.Net;
using RunJit.Cli.Services;

namespace RunJit.Cli.RunJit.Update.CodeRules
{
    internal static class AddCloneReposAndUpdateAllExtension
    {
        internal static void AddCloneReposAndUpdateAll(this IServiceCollection services)
        {
            services.AddConsoleService();
            services.AddGitService();
            services.AddDotNet();
            services.AddAwsCodeCommit();
            services.AddFindSolutionFile();

            services.AddSingletonIfNotExists<IUpdateCodeRulesStrategy, CloneReposAndUpdateAll>();
        }
    }

    internal class CloneReposAndUpdateAll(IConsoleService consoleService,
                                          IGitService git,
                                          IDotNet dotNet,
                                          IRenameFilesAndFolders renameFilesAndFolders,
                                          // IUpdateNugetPackageService updateNugetPackageService,
                                          FindSolutionFile findSolutionFile,
                                          IMediator mediator,
                                          IHttpClientFactory httpClientFactory,
                                          IRunJitApiClientFactory runJitApiClientFactory,
                                          RunJitApiClientSettings runJitApiClientSettings) : IUpdateCodeRulesStrategy
    {
        public bool CanHandle(UpdateCodeRulesParameters parameters)
        {
            return parameters.SolutionFile.IsNullOrWhiteSpace() &&
                   parameters.GitRepos.IsNotNullOrWhiteSpace();
        }

        public async Task HandleAsync(UpdateCodeRulesParameters parameters)
        {
            // 0. Check that precondition is met
            if (CanHandle(parameters).IsFalse())
            {
                throw new RunJitException($"Please call {nameof(IUpdateCodeRulesStrategy.CanHandle)} before call {nameof(IUpdateCodeRulesStrategy.HandleAsync)}");
            }
            
            if (parameters.Branch.IsNullOrWhiteSpace())
            {
                throw new RunJitException($"Please provider a branch name which have to be used as base for integrating the code rules.");
            }

            // 1. Check if solution file is the file or directory
            //    if it is null or whitespace we check current directory
            var repos = parameters.GitRepos.Split(';');
            var orginalStartFolder = parameters.WorkingDirectory.IsNotNullOrWhiteSpace() ? parameters.WorkingDirectory : Environment.CurrentDirectory;
            if (Directory.Exists(orginalStartFolder).IsFalse())
            {
                Directory.CreateDirectory(orginalStartFolder);
            }
            
            
            foreach (var repo in repos)
            {
                var index = repos.IndexOf(repo) + 1;
                consoleService.WriteSuccess($"Start upgrading code rules for repo {index} of {repos.Length}");

                // 2. Get created git folder
                var folder = repo.Split("/").Last().Replace(".git", string.Empty);
                var currentRepoEnvironment = Path.Combine(orginalStartFolder, folder);
                var targetDirectoryInfo = new DirectoryInfo(currentRepoEnvironment);
                if (targetDirectoryInfo.NotExists())
                {
                    targetDirectoryInfo.Create();
                }

                Environment.CurrentDirectory = targetDirectoryInfo.FullName;
                
                // 1. Git clone
                await git.CloneAsync(repo, parameters.Branch, targetDirectoryInfo).ConfigureAwait(false);
                await git.FetchAsync().ConfigureAwait(false);


                // NEW check for legacy branches and delete them all
                var branches = await git.GetRemoteBranchesAsync().ConfigureAwait(false);
                var branchName = "quality/update-coderules";

                var legacyBranches = branches.Where(b => b.Name.Contains(branchName, StringComparison.OrdinalIgnoreCase)).ToImmutableList();

                await git.DeleteBranchesAsync(legacyBranches).ConfigureAwait(false);

                // 4. Create new branch, check that branch is unique
                var qualityUpdateCodeRulesPackages = branchName;

                await git.CreateBranchAsync(qualityUpdateCodeRulesPackages).ConfigureAwait(false);

                // 5. Check if solution file is the file or directory
                //    if it is null or whitespace we check current directory 
                var solutionFile = findSolutionFile.Find(Environment.CurrentDirectory);
                var solutionNameNormalized = solutionFile.NameWithoutExtension().Split('.').Select(part => part.FirstCharToUpper()).Flatten(".");

                // 6. Build the solution first
                await dotNet.BuildAsync(solutionFile).ConfigureAwait(false);

                //// 7. Get infos which packages are outdated
                //var outdatedNugetTargetSolution = await dotNet.ListOutdatedPackagesAsync(solutionFile).ConfigureAwait(false);

                //// 8. Update the nuget packages
                //await updateNugetPackageService.UpdateNugetPackageAsync(outdatedNugetTargetSolution, parameters.IgnorePackages.Split(";").ToImmutableList()).ConfigureAwait(false);

                // Create temp folder for fetching and renaming and using
                var combine = Path.Combine(solutionFile.Directory!.FullName, Guid.NewGuid().ToString().ToLowerInvariant());
                var tempFolder = new DirectoryInfo(combine);
                tempFolder.Create();
                Environment.CurrentDirectory = tempFolder.FullName;

                var auth = await mediator.SendAsync(new GetTokenByStorageCache()).ConfigureAwait(false);
                var httpClient = httpClientFactory.CreateClient();
                httpClient.BaseAddress = new Uri(runJitApiClientSettings.BaseAddress);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(auth.TokenType, auth.Token);
                var rRunJitApiClient = runJitApiClientFactory.CreateFrom(httpClient);

                var codeRuleAsFileStream = await rRunJitApiClient.CodeRules.V1.ExportCodeRulesAsync().ConfigureAwait(false);
                using var zipArchive = new ZipArchive(codeRuleAsFileStream.FileStream, ZipArchiveMode.Read);
                zipArchive.ExtractToDirectory(tempFolder.FullName);

                //// 5. Check if solution file is the file or directory
                ////    if it is null or whitespace we check current directory 
                //var codeRuleSolution = findSolutionFile.Find(tempFolder.FullName);

                //// 6. Build the solution first
                //await dotNet.BuildAsync(codeRuleSolution).ConfigureAwait(false);

                //// 7. Get infos which packages are outdated
                //var outdatedNugetCodeRuleSolution = await dotNet.ListOutdatedPackagesAsync(codeRuleSolution).ConfigureAwait(false);

                //// 8. Update the nuget packages
                //await updateNugetPackageService.UpdateNugetPackageAsync(outdatedNugetCodeRuleSolution, parameters.IgnorePackages.Split(";").ToImmutableList()).ConfigureAwait(false);

                Environment.CurrentDirectory = currentRepoEnvironment;

                //foreach (var file in tempFolder.EnumerateFiles("*.cs", SearchOption.AllDirectories))
                //{
                //    var fileContent = await File.ReadAllTextAsync(file.FullName);
                //    if (fileContent.Contains("RunJit.CodeRules.sln"))
                //    {
                //        var newFileContent = fileContent.Replace("RunJit.CodeRules.sln", $"{solutionNameNormalized}.sln");
                //        await File.WriteAllTextAsync(file.FullName, newFileContent);
                //    }
                //}

                var alreadyExistingCodeRuleFolder = solutionFile.Directory!.EnumerateDirectories("*.CodeRules").FirstOrDefault();
                if (alreadyExistingCodeRuleFolder.IsNotNull())
                {
                    alreadyExistingCodeRuleFolder.Delete(true);
                }

                var codeRuleFolder = tempFolder.EnumerateDirectories("*.CodeRules", SearchOption.AllDirectories).FirstOrDefault();
                if (codeRuleFolder.IsNull())
                {
                    throw new FileNotFoundException($"Could not find the code rules folder RunJit.CodeRules");
                }

                var newTarget = new DirectoryInfo(Path.Combine(solutionFile.Directory!.FullName, codeRuleFolder.Name));
                codeRuleFolder.MoveTo(newTarget.FullName);

                var projectNameFromCodeRules = codeRuleFolder.Name.Split(".")[^2];

                var renamedDirectory = renameFilesAndFolders.Rename(newTarget, projectNameFromCodeRules, solutionNameNormalized);

                consoleService.WriteSuccess($"New code rule folder: {renamedDirectory.FullName}");

                var newCodeRuleProject = renamedDirectory.EnumerateFiles("*.csproj").FirstOrDefault();
                if (newCodeRuleProject.IsNull())
                {
                    throw new FileNotFoundException($"Could not find the code rules project after renaming process.");
                }

                tempFolder.Delete(true);

                var allCodeRuleCsprojs = solutionFile.Directory.EnumerateFiles("*CodeRule*.csproj", SearchOption.AllDirectories).ToList();
                foreach (var allCodeRuleCsproj in allCodeRuleCsprojs)
                {
                    await dotNet.AddProjectToSolutionAsync(solutionFile, allCodeRuleCsproj);
                }


                // 6. Build the solution first
                await dotNet.BuildAsync(solutionFile).ConfigureAwait(false);

                // 7. Get infos which packages are outdated
                // var outdatedCodeRulesResponse = await dotNet.ListOutdatedPackagesAsync(solutionFile).ConfigureAwait(false);

                //  await updateCodeRulesPackageService.UpdateCodeRulesPackageAsync(outdatedCodeRulesResponse).ConfigureAwait(false);

                // 9.Add changes to git
                await git.AddAsync().ConfigureAwait(false);

                // 10. Commit changes
                await git.CommitAsync("Update coderules packages").ConfigureAwait(false);

                // 11. Push changes
                await git.PushAsync(branchName).ConfigureAwait(false);

                //// 12. Create pull request
                //await awsCodeCommit.CreatePullRequestAsync("Update coderules packages",
                //                                           "Update coderules packages to the newest versions",
                //                                           qualityUpdateCodeRulesPackages).ConfigureAwait(false);


                consoleService.WriteSuccess($"Solution: {solutionFile.FullName} was successfully update to the newest coderules packages");
            }
        }
    }
}
