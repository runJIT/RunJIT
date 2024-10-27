using System.Collections.Immutable;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.ErrorHandling;
using RunJit.Cli.RunJit.Update.Net;
using RunJit.Cli.Services;
using RunJit.Cli.Services.AwsCodeCommit;
using RunJit.Cli.Services.Git;
using RunJit.Cli.Services.Net;

namespace RunJit.Cli.RunJit.Update.Nuget
{
    internal static class AddUpdateLocalSolutionFileExtension
    {
        internal static void AddUpdateLocalSolutionFile(this IServiceCollection services)
        {
            services.AddConsoleService();
            services.AddGitService();
            services.AddDotNet();
            services.AddDotNetService();
            services.AddUpdateNugetPackageService();
            services.AddFindSolutionFile();

            services.AddSingletonIfNotExists<IUpdateNugetStrategy, UpdateLocalSolutionFile>();
        }
    }

    internal sealed class UpdateLocalSolutionFile(ConsoleService consoleService,
                                           IGitService git,
                                           IAwsCodeCommit awsCodeCommit,
                                           IDotNet dotNet,
                                           IUpdateNugetPackageService updateNugetPackageService,
                                           FindSolutionFile findSolutionFile) : IUpdateNugetStrategy
    {
        public bool CanHandle(UpdateNugetParameters parameters)
        {
            return parameters.SolutionFile.IsNotNullOrWhiteSpace();
        }

        public async Task HandleAsync(UpdateNugetParameters parameters)
        {
            // 0. Check that precondition is met
            if (CanHandle(parameters).IsFalse())
            {
                throw new RunJitException($"Please call {nameof(IUpdateNugetStrategy.CanHandle)} before call {nameof(IUpdateNugetStrategy.HandleAsync)}");
            }

            // 1. Check if solution file is the file or directory
            //    if it is null or whitespace we check current directory
            var solutionFile = findSolutionFile.Find(parameters.SolutionFile);

            // 2. Set current directory to solution file directory - cause of git commands and more
            Environment.CurrentDirectory = solutionFile.Directory!.FullName;

            // 3. Create new branch - only if git exists
            var branchName = "quality/update-nuget-packages";

            // 4. Check if git exists
            var existingGitFolder = solutionFile.Directory!.EnumerateDirectories(".git").FirstOrDefault();

            if (existingGitFolder.IsNotNull())
            {
                // NEW check for legacy branches and delete them all
                var branches = await git.GetRemoteBranchesAsync().ConfigureAwait(false);
                var legacyBranches = branches.Where(b => b.Name.Contains(branchName, StringComparison.OrdinalIgnoreCase)).ToImmutableList();

                await git.DeleteBranchesAsync(legacyBranches).ConfigureAwait(false);

                await git.CreateBranchAsync(branchName).ConfigureAwait(false);
            }

            // 7. Build solution first to go sure anything is working
            await dotNet.BuildAsync(solutionFile).ConfigureAwait(false);

            // 8. Get outdated nuget packages
            var outdatedNugetResponse = await dotNet.ListOutdatedPackagesAsync(solutionFile).ConfigureAwait(false);

            // 9. Update nuget packages
            await updateNugetPackageService.UpdateNugetPackageAsync(outdatedNugetResponse, parameters.IgnorePackages.Split(";").ToImmutableList()).ConfigureAwait(false);

            if (existingGitFolder.IsNotNull())
            {
                // 10. Add git changes
                await git.AddAsync().ConfigureAwait(false);

                // 11. Commit git changes
                await git.CommitAsync("Update nuget packages").ConfigureAwait(false);

                // 12. Push git changes
                //     We only push if the git folder exists
                await git.PushAsync(branchName).ConfigureAwait(false);

                // 13. Create pull request in aws code commit
                await awsCodeCommit.CreatePullRequestAsync("Update nuget packages",
                                                           "Update nuget packages to the newest versions",
                                                           branchName).ConfigureAwait(false);
            }

            consoleService.WriteSuccess($"Solution: {solutionFile.FullName} was successfully update to the newest nuget packages");
        }
    }
}
