using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.New.MinimalApiProject.CodeGen.github.ISSUE_TEMPLATE;
using RunJit.Cli.New.MinimalApiProject.CodeGen.github.PULL_REQUEST_TEMPLATE;
using RunJit.Cli.New.MinimalApiProject.CodeGen.github.workflows;
using RunJit.Cli.Services;
using Solution.Parser.Solution;

namespace RunJit.Cli.New.MinimalApiProject.CodeGen.github
{
    internal static class AddGitHubCodeGenExtension
    {
        internal static void AddGitHubCodeGen(this IServiceCollection services)
        {
            // Issue Templates
            services.AddBugReportCodeGen();
            services.AddFeatureRequestCodeGen();
            services.AddSupportQuestionCodeGen();

            // Pull Request Templates
            services.AddPullRequestTemplateCodeGen();

            // Workflows
            services.AddWorkflowCodeGen();

            services.AddSingletonIfNotExists<IMinimalApiProjectRootLevelCodeGen, GitHubCodeGen>();
        }
    }

    internal interface IGitHubCodeGen
    {
        Task GenerateAsync(SolutionFile projectFileInfo,
                           DirectoryInfo gitHubFolder,
                           MinimalApiProjectInfos minimalApiProjectInfos);
    }

    internal sealed class GitHubCodeGen(ConsoleService consoleService,
                                        IEnumerable<IGitHubCodeGen> gitHubCodeGens) : IMinimalApiProjectRootLevelCodeGen
    {
        public async Task GenerateAsync(SolutionFile solutionFile,
                                        MinimalApiProjectInfos minimalApiProjectInfos)
        {
            // 1. .github folder
            var gitHubFolder = new DirectoryInfo(Path.Combine(solutionFile.SolutionFileInfo.Value.Directory!.FullName, ".github"));
            if (gitHubFolder.NotExists())
            {
                gitHubFolder.Create();
            }

            // 2. Add App.cs
            foreach (var gitHubCodeGen in gitHubCodeGens)
            {
                await gitHubCodeGen.GenerateAsync(solutionFile, gitHubFolder, minimalApiProjectInfos).ConfigureAwait(false);
            }

            consoleService.WriteSuccess(".github folder part successfully created");
        }
    }
}
