using System.Collections.Immutable;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.AwsCodeCommit;
using RunJit.Cli.ErrorHandling;
using RunJit.Cli.Git;
using RunJit.Cli.Net;
using RunJit.Cli.RunJit.Update.Backend.Nuget;
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
                                          IAwsCodeCommit awsCodeCommit,
                                          IRenameFilesAndFolders renameFilesAndFolders,
                                          IUpdateNugetPackageService updateNugetPackageService,
                                          FindSolutionFile findSolutionFile) : IUpdateCodeRulesStrategy
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

            // 1. Check if solution file is the file or directory
            //    if it is null or whitespace we check current directory
            var repos = parameters.GitRepos.Split(';');
            var orginalStartFolder = parameters.WorkingDirectory.IsNotNullOrWhiteSpace() ? parameters.WorkingDirectory : Environment.CurrentDirectory;

            var codeRulesGitRepo = "codecommit::eu-central-1://pulse-code-rules"; // ToDo: Neutral code rules repository

            foreach (var repo in repos)
            {
                var index = repos.IndexOf(repo) + 1;
                consoleService.WriteSuccess($"Start upgrading code rules for repo {index} of {repos.Length}");

                Environment.CurrentDirectory = orginalStartFolder;

                // 1. Git clone
                await git.CloneAsync(repo).ConfigureAwait(false);

                // 2. Get created git folder
                var folder = repo.Split("//").Last();
                var currentRepoEnvironment = Path.Combine(orginalStartFolder, folder);
                Environment.CurrentDirectory = currentRepoEnvironment;

                // 3. Checkout master branch
                await git.CheckoutAsync("master").ConfigureAwait(false);

                // NEW check for legacy branches and delete them all
                var branches = await git.GetRemoteBranchesAsync().ConfigureAwait(false);
                var branchName = "quality/update-coderules-packages";

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

                // 7. Get infos which packages are outdated
                var outdatedNugetTargetSolution = await dotNet.ListOutdatedPackagesAsync(solutionFile).ConfigureAwait(false);

                // 8. Update the nuget packages
                await updateNugetPackageService.UpdateNugetPackageAsync(outdatedNugetTargetSolution, parameters.IgnorePackages.Split(";").ToImmutableList()).ConfigureAwait(false);

                // Create temp folder for fetching and renaming and using
                var combine = Path.Combine(solutionFile.Directory!.FullName, Guid.NewGuid().ToString().ToLowerInvariant());
                var tempFolder = new DirectoryInfo(combine);
                tempFolder.Create();
                Environment.CurrentDirectory = tempFolder.FullName;

                await git.CloneAsync(codeRulesGitRepo).ConfigureAwait(false);

                var gitFolder = tempFolder.EnumerateDirectories(".git", SearchOption.AllDirectories).FirstOrDefault();
                if (gitFolder.IsNull())
                {
                    throw new FileNotFoundException($"Could not find the .git folder from the cloned code rule repository: {codeRulesGitRepo}");
                }

                gitFolder.Attributes = FileAttributes.Normal;
                foreach (var info in gitFolder.GetFileSystemInfos("*", SearchOption.AllDirectories))
                {
                    info.Attributes = FileAttributes.Normal;
                }

                gitFolder.Delete(true);


                // 5. Check if solution file is the file or directory
                //    if it is null or whitespace we check current directory 
                var codeRuleSolution = findSolutionFile.Find(tempFolder.FullName);

                // 6. Build the solution first
                await dotNet.BuildAsync(codeRuleSolution).ConfigureAwait(false);

                // 7. Get infos which packages are outdated
                var outdatedNugetCodeRuleSolution = await dotNet.ListOutdatedPackagesAsync(codeRuleSolution).ConfigureAwait(false);

                // 8. Update the nuget packages
                await updateNugetPackageService.UpdateNugetPackageAsync(outdatedNugetCodeRuleSolution, parameters.IgnorePackages.Split(";").ToImmutableList()).ConfigureAwait(false);

                Environment.CurrentDirectory = currentRepoEnvironment;


                foreach (var file in tempFolder.EnumerateFiles("*.cs", SearchOption.AllDirectories))
                {
                    var fileContent = await File.ReadAllTextAsync(file.FullName);
                    if (fileContent.Contains("RunJit.CodeRules.sln"))
                    {
                        var newFileContent = fileContent.Replace("RunJit.CodeRules.sln", $"{solutionNameNormalized}.sln");
                        await File.WriteAllTextAsync(file.FullName, newFileContent);
                    }
                }

                var alreadyExistingCodeRuleFolder = solutionFile.Directory!.EnumerateDirectories("*.CodeRules", SearchOption.AllDirectories).FirstOrDefault();
                if (alreadyExistingCodeRuleFolder.IsNotNull())
                {
                    alreadyExistingCodeRuleFolder.Delete(true);
                }

                var codeRuleFolder = tempFolder.EnumerateDirectories("RunJit.CodeRules", SearchOption.AllDirectories).FirstOrDefault();
                if (codeRuleFolder.IsNull())
                {
                    throw new FileNotFoundException($"Could not find the code rules folder from the cloned code rule repository: {codeRulesGitRepo}");
                }

                var newTarget = new DirectoryInfo(Path.Combine(solutionFile.Directory!.FullName, codeRuleFolder.Name));
                codeRuleFolder.MoveTo(newTarget.FullName);

                var renamedDirectory = renameFilesAndFolders.Rename(newTarget, "RunJit.CodeRules", $"{solutionNameNormalized}.CodeRules");

                consoleService.WriteSuccess($"New code rule folder: {renamedDirectory.FullName}");

                var newCodeRuleProject = renamedDirectory.EnumerateFiles("*.csproj").FirstOrDefault();
                if (newCodeRuleProject.IsNull())
                {
                    throw new FileNotFoundException($"Could not find the code rules project after renaming process.");
                }

                await dotNet.AddProjectToSolutionAsync(solutionFile, newCodeRuleProject);

                tempFolder.Delete(true);

                // 6. Build the solution first
                // await dotNet.BuildAsync(solutionFile).ConfigureAwait(false);

                //// 7. Get infos which packages are outdated
                //var outdatedCodeRulesResponse = await dotNet.ListOutdatedPackagesAsync(solutionFile).ConfigureAwait(false);

                // 8. Update the coderules packages
                // await updateCodeRulesPackageService.UpdateCodeRulesPackageAsync(outdatedCodeRulesResponse).ConfigureAwait(false);

                // 9. Add changes to git
                await git.AddAsync().ConfigureAwait(false);

                // 10. Commit changes
                await git.CommitAsync("Update coderules packages").ConfigureAwait(false);

                // 11. Push changes
                await git.PushAsync(qualityUpdateCodeRulesPackages).ConfigureAwait(false);

                // 12. Create pull request
                await awsCodeCommit.CreatePullRequestAsync("Update coderules packages",
                                                           "Update coderules packages to the newest versions",
                                                           qualityUpdateCodeRulesPackages).ConfigureAwait(false);

                consoleService.WriteSuccess($"Solution: {solutionFile.FullName} was successfully update to the newest coderules packages");
            }
        }
    }
}
