﻿using System.Collections.Immutable;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.AwsCodeCommit;
using RunJit.Cli.ErrorHandling;
using RunJit.Cli.Git;
using RunJit.Cli.Net;
using RunJit.Cli.Services;

namespace RunJit.Cli.RunJit.Check.Backend.Builds
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

            services.AddSingletonIfNotExists<ICheckBackendBuildsStrategy, CloneReposAndUpdateAll>();
        }
    }

    internal class CloneReposAndUpdateAll(IConsoleService consoleService,
                                          IGitService git,
                                          IDotNet dotNet,
                                          IAwsCodeCommit awsCodeCommit,
                                          FindSolutionFile findSolutionFile) : ICheckBackendBuildsStrategy
    {
        public bool CanHandle(CheckBackendBuildsParameters parameters)
        {
            return parameters.SolutionFile.IsNullOrWhiteSpace() &&
                   parameters.GitRepos.IsNotNullOrWhiteSpace();
        }

        public async Task HandleAsync(CheckBackendBuildsParameters parameters)
        {
            // 0. Check that precondition is met
            if (CanHandle(parameters).IsFalse())
            {
                throw new RunJitException($"Please call {nameof(ICheckBackendBuildsStrategy.CanHandle)} before call {nameof(ICheckBackendBuildsStrategy.HandleAsync)}");
            }

            // 1. Check if solution file is the file or directory
            //    if it is null or whitespace we check current directory
            var repos = parameters.GitRepos.Split(';');
            var orginalStartFolder = parameters.WorkingDirectory.IsNotNullOrWhiteSpace() ? parameters.WorkingDirectory : Environment.CurrentDirectory;

            foreach (var repo in repos)
            {
                var index = repos.IndexOf(repo) + 1;
                consoleService.WriteSuccess($"Try checking if backends are build-able. Backend {index} of {repos.Length}");

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
                var branchName = "quality/ms-build-check";
                
                var legacyBranches = branches.Where(b => b.Name.Contains(branchName,StringComparison.OrdinalIgnoreCase)).ToImmutableList();
                
                await git.DeleteBranchesAsync(legacyBranches).ConfigureAwait(false);
                
                // 4. Create new branch, check that branch is unique
                var qualityCheckBackendBuildsPackages = branchName;

                await git.CreateBranchAsync(qualityCheckBackendBuildsPackages).ConfigureAwait(false);

                // 5. Check if solution file is the file or directory
                //    if it is null or whitespace we check current directory 
                var solutionFile = findSolutionFile.Find(Environment.CurrentDirectory);

                // 6. Build the solution first
                var tryBuildResult = await dotNet.TryBuildAsync(solutionFile).ConfigureAwait(false);

                // if backend build fails, we need to fix it.
                if (tryBuildResult.WasSuccessful.IsFalse())
                {
                    // Create error txt -> hint pipeline can be green cause of different net versions !
                    var errorFile = Path.Combine(solutionFile.Directory!.FullName, "build-errors.txt");
                    await File.WriteAllTextAsync(errorFile, tryBuildResult.Message);

                    // 7. Add changes to git
                    await git.AddAsync().ConfigureAwait(false);

                    // 8. Commit changes
                    await git.CommitAsync("Build failed").ConfigureAwait(false);

                    // 9. Push changes
                    await git.PushAsync(qualityCheckBackendBuildsPackages).ConfigureAwait(false);

                    // 10. Create pull request
                    await awsCodeCommit.CreatePullRequestAsync("Build failed please check.",
                                                               "Build failed. Please check the build errors.", qualityCheckBackendBuildsPackages).ConfigureAwait(false);
                }

                consoleService.WriteSuccess($"Solution: {solutionFile.FullName} was successfully checked and was buildable");
            }
        }
    }
}
