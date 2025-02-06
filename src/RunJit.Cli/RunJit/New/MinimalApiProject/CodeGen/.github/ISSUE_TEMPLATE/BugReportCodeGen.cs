﻿using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;
using Solution.Parser.Solution;

namespace RunJit.Cli.New.MinimalApiProject.CodeGen.github.ISSUE_TEMPLATE
{
    internal static class AddBugReportCodeGenExtension
    {
        internal static void AddBugReportCodeGen(this IServiceCollection services)
        {
            services.AddConsoleService();
            services.AddNamespaceProvider();

            services.AddSingletonIfNotExists<IGitHubCodeGen, BugReportCodeGen>();
        }
    }

    internal sealed class BugReportCodeGen(ConsoleService consoleService) : IGitHubCodeGen
    {
        private const string Template = """
                                        ---
                                        name: "\U0001F41B Bug Report"
                                        about: "If something isn't working as expected \U0001F914."
                                        title: ''
                                        labels: 'i: bug, i: needs triage'
                                        assignees: ''

                                        ---

                                        ## Bug Report

                                        **Current Behavior**
                                        A clear and concise description of the behavior. Put in links and as much information you have about the Bug.

                                        **Expected behavior/code**
                                        A clear and concise description of what you expected to happen (or code).

                                        **Environment**
                                        - Tenant: [e.g. `siemens`]
                                        - How you are using SDC: [e.g. `cli`, `api`, `frontend`]

                                        **Possible Solution**
                                        <!--- Only if you have suggestions on a fix for the bug -->

                                        **Additional context/Screenshots**
                                        Add any other context about the problem here. If applicable, add screenshots to help explain.
                                        """;


        public async Task GenerateAsync(SolutionFile projectFileInfo,
                                        DirectoryInfo gitHubFolder,
                                        MinimalApiProjectInfos minimalApiProjectInfos)
        {
            // 1. .github folder
            var issueTemplateFolder = new DirectoryInfo(Path.Combine(gitHubFolder.FullName, "ISSUE_TEMPLATE"));
            if (issueTemplateFolder.NotExists())
            {
                issueTemplateFolder.Create();
            }

            // 2. Write all templates
            var file = Path.Combine(issueTemplateFolder.FullName, "Bug_report.md");

            await File.WriteAllTextAsync(file, Template).ConfigureAwait(false);

            // 3. Print success message
            consoleService.WriteSuccess($"Successfully created {file}");
        }
    }
}
