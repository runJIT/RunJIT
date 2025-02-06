﻿using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;
using Solution.Parser.Solution;

namespace RunJit.Cli.New.MinimalApiProject.CodeGen
{
    internal static class AddContributingCodeGenExtension
    {
        internal static void AddContributingCodeGen(this IServiceCollection services)
        {
            services.AddConsoleService();
            services.AddNamespaceProvider();

            services.AddSingletonIfNotExists<IMinimalApiProjectRootLevelCodeGen, ContributingCodeGen>();
        }
    }

    internal sealed class ContributingCodeGen(ConsoleService consoleService) : IMinimalApiProjectRootLevelCodeGen
    {
        private const string Template = """
                                        # Contributing
                                        
                                        We welcome contributions in several forms, e.g.
                                        
                                        * Sponsoring
                                        * Documenting
                                        * Testing
                                        * Coding
                                        * etc.
                                        
                                        Please check [Issues](https://github.siemens.cloud/sdc/siemens-data-cloud-core/issues) and look for
                                        unassigned ones or create a new one.
                                        
                                        Working together in an open and welcoming environment is the foundation of our
                                        success, so please respect our [Code of Conduct](./CODE_OF_CONDUCT.md).
                                        
                                        ## Guidelines
                                        
                                        ### Workflow
                                        
                                        We use the
                                        [Feature Branch Workflow](https://www.atlassian.com/git/tutorials/comparing-workflows/feature-branch-workflow)
                                        and review all changes we merge to the main branch.
                                        
                                        We appreciate any contributions, so please use the [Forking Workflow](https://www.atlassian.com/git/tutorials/comparing-workflows/forking-workflow)
                                        and send us [Merge Requests](https://docs.gitlab.com/17.3/ee/user/project/merge_requests/index.html)!
                                        If You plan to contribute regularly, please consider participating in our
                                        [Ambassadors](./_ambassadors.md) team - ambassadors get developer access to
                                        this project automatically.
                                        
                                        ### Commit Message
                                        
                                        Commit messages shall follow the conventions defined by [conventional-changelog](https://wiki.siemens.com/display/en/Conventional+Changelog), for example:
                                        
                                        * `docs(security): add dockle security chapter`
                                        * `fix(theme): properly size and position hero img`
                                        * `style(markdown): run markdownlint auto-fix with latest version`
                                        
                                        If you accidentally pushed a commit with a malformed message you have to [reword the commit](https://docs.github.com/en/pull-requests/committing-changes-to-your-project/creating-and-editing-commits/changing-a-commit-message).
                                        
                                        #### Enforcing Proper Commit Messages
                                        
                                        We use [commitlint](https://plugins.jetbrains.com/plugin/14046-commitlint-conventional-commit/versions#tabs) to automatically validate commit messages,
                                        allowing us to enforce the [conventional-changelog](https://wiki.siemens.com/display/en/Conventional+Changelog).
                                        
                                        
                                        
                                        """;


        public async Task GenerateAsync(SolutionFile solutionFile,
                                        MinimalApiProjectInfos minimalApiProjectInfos)
        {
            // 1. Setup file name
            var file = Path.Combine(solutionFile.SolutionFileInfo.Value.Directory!.FullName, "CONTRIBUTING.md");

            // 2. Write file
            await File.WriteAllTextAsync(file, Template).ConfigureAwait(false);

            // 3. Print success message
            consoleService.WriteSuccess($"Successfully created {file}");
        }
    }
}
