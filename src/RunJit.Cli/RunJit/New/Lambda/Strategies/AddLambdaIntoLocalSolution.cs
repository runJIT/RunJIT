using System.Collections.Immutable;
using System.IO.Compression;
using System.Net.Http.Headers;
using AspNetCore.Simple.Sdk.Mediator;
using Extensions.Pack;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Api.Client;
using RunJit.Cli.Auth0;
using RunJit.Cli.AwsCodeCommit;
using RunJit.Cli.ErrorHandling;
using RunJit.Cli.Git;
using RunJit.Cli.Net;
using RunJit.Cli.RunJit.Update.CodeRules;
using RunJit.Cli.RunJit.Update.Net;
using RunJit.Cli.RunJit.Update.Nuget;
using RunJit.Cli.Services;

namespace RunJit.Cli.RunJit.New.Lambda
{
    internal static class AddAddLambdaIntoLocalSolutionExtension
    {
        internal static void AddNewLambdaIntoLocalSolution(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddConsoleService();
            services.AddGitService();
            services.AddDotNet();
            services.AddDotNetService();
            services.AddAwsCodeCommit();
            services.AddUpdateNugetPackageService();
            services.AddRenameFilesAndFolders();
            services.AddFindSolutionFile();


            services.AddRunJitApiClientFactory(configuration);
            services.AddRunJitApiClient();
            services.AddHttpClient();
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(AddAddLambdaIntoLocalSolutionExtension).Assembly);
            });

            
            services.AddFindSourceFolder();
            
            services.AddSingletonIfNotExists<IAddNewLambdaServiceStrategy, AddLambdaIntoLocalSolution>();
        }
    }

    internal class AddLambdaIntoLocalSolution(IConsoleService consoleService,
                                              IGitService git,
                                              //IAwsCodeCommit awsCodeCommit,
                                              IDotNet dotNet,
                                              //IUpdateNugetPackageService updateNugetPackageService,
                                              IRenameFilesAndFolders renameFilesAndFolders,
                                              FindSolutionFile findSolutionFile,
                                              IRunJitApiClientFactory runJitApiClientFactory,
                                              IHttpClientFactory httpClientFactory,
                                              IMediator mediator,
                                              RunJitApiClientSettings runJitApiClientSettings,
                                              FindSourceFolder findSourceFolder) : IAddNewLambdaServiceStrategy
    {
        public bool CanHandle(LambdaParameters parameters)
        {
            return parameters.Solution.Exists;
        }

        public async Task HandleAsync(LambdaParameters parameters)
        {
            // 0. Check that precondition is met
            if (CanHandle(parameters).IsFalse())
            {
                throw new RunJitException($"Please call {nameof(IUpdateCodeRulesStrategy.CanHandle)} before call {nameof(IUpdateCodeRulesStrategy.HandleAsync)}");
            }

            // 5. Check if solution file is the file or directory
            //    if it is null or whitespace we check current directory 
            var solutionFile = findSolutionFile.Find(parameters.Solution.FullName);

            var branchName = "feature/new-lambda";

            // 4. Check if git exists
            var existingGitFolder = solutionFile.Directory!.EnumerateDirectories(".git").FirstOrDefault();
            if (existingGitFolder.IsNotNull())
            {
                Environment.CurrentDirectory = solutionFile.Directory!.FullName;

                // NEW check for legacy branches and delete them all
                var branches = await git.GetRemoteBranchesAsync().ConfigureAwait(false);
                var localBranches = await git.GetLocalBranchesAsync().ConfigureAwait(false);

                if (localBranches.Any(b => b.IsActiveBranch && b.Name == branchName).IsFalse())
                {
                    var legacyBranches = branches.Where(b => b.Name.Contains(branchName, StringComparison.OrdinalIgnoreCase)).ToImmutableList();

                    await git.DeleteBranchesAsync(legacyBranches).ConfigureAwait(false);

                    await git.CreateBranchAsync(branchName).ConfigureAwait(false);
                }
            }

            var currentRepoEnvironment = solutionFile.Directory!.FullName;
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

            var auth = await mediator.SendAsync(new GetTokenByStorageCache()).ConfigureAwait(false);
            var httpClient = httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(runJitApiClientSettings.BaseAddress);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(auth.TokenType, auth.Token);
            var rRunJitApiClient = runJitApiClientFactory.CreateFrom(httpClient);

            var codeRuleAsFileStream = await rRunJitApiClient.Lambdas.V1.CreateLambdaAsync().ConfigureAwait(false);
            using var zipArchive = new ZipArchive(codeRuleAsFileStream.FileStream, ZipArchiveMode.Read);
            zipArchive.ExtractToDirectory(tempFolder.FullName);

            foreach (var enumerateDirectory in tempFolder.EnumerateDirectories())
            {
                // 3. replace placeholders
                // Solution and projects
                var projectName = ExtractProjectName(parameters);
                renameFilesAndFolders.Rename(enumerateDirectory, "rps.template", projectName);

                renameFilesAndFolders.Rename(enumerateDirectory, "$lambda-name$", parameters.LambdaName);

                // DotNetTool name
                // class etc. and rest
                renameFilesAndFolders.Rename(enumerateDirectory, "Rps", parameters.FunctionName);

                renameFilesAndFolders.Rename(enumerateDirectory, "$module$", parameters.ModuleName);
            }

            var sourceFolder = findSourceFolder.GetTargetSourceFolder(solutionFile);
            
            foreach (var directory in tempFolder.EnumerateDirectories())
            {
                var newTarget = new DirectoryInfo(Path.Combine(sourceFolder.FullName, directory.Name));
                directory.MoveTo(newTarget.FullName);
                
                var csprojs = newTarget.EnumerateFiles("*.csproj", SearchOption.TopDirectoryOnly);
                foreach (var projectFile in csprojs)
                {
                    await dotNet.AddProjectToSolutionAsync(solutionFile, projectFile);    
                }
            }
            
            tempFolder.Delete(true);
            
            // 6. Build the solution first
            await dotNet.BuildAsync(solutionFile).ConfigureAwait(false);

            // 7. Get infos which packages are outdated
            // var outdatedCodeRulesResponse = await dotNet.ListOutdatedPackagesAsync(solutionFile).ConfigureAwait(false);

            //  await updateCodeRulesPackageService.UpdateCodeRulesPackageAsync(outdatedCodeRulesResponse).ConfigureAwait(false);

            if (existingGitFolder.IsNotNull())
            {
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
            }

            consoleService.WriteSuccess($"Solution: {solutionFile.FullName} was successfully update to the newest code rules");
        }
        
        private static string ExtractProjectName(LambdaParameters parameters)
        {
            return parameters.LambdaName.Split("-").Select(word => word.FirstCharToUpper()).Flatten(".");
        }
    }
}
