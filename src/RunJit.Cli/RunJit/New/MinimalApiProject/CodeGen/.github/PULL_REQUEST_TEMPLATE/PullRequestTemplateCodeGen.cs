using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;
using Solution.Parser.Solution;

namespace RunJit.Cli.New.MinimalApiProject.CodeGen.github.PULL_REQUEST_TEMPLATE
{
    internal static class AddPullRequestTemplateCodeGenExtension
    {
        internal static void AddPullRequestTemplateCodeGen(this IServiceCollection services)
        {
            services.AddConsoleService();
            services.AddNamespaceProvider();

            services.AddSingletonIfNotExists<IGitHubCodeGen, PullRequestTemplateCodeGen>();
        }
    }

    internal sealed class PullRequestTemplateCodeGen(ConsoleService consoleService) : IGitHubCodeGen
    {
        private const string Template = """
                                        ## Describe your changes
                                        
                                        ## Issue ticket number and link
                                        
                                        ## Bugfix Checklist
                                        - [ ] I have performed a self-review of my code
                                        - [ ] I have added thorough tests / adapted tests
                                        - [ ] All Code Rules are green
                                        - [ ] I used conventional commit messages for all commits
                                        - [ ] I adapted the Changelist file with my fix
                                        
                                        ## Feature Checklist
                                        - [ ] I have performed a self-review of my code
                                        - [ ] I have added thorough tests / adapted tests
                                        - [ ] All Code Rules are green
                                        - [ ] I used conventional commit messages for all commits
                                        - [ ] I adapted the Changelist file with the description of the new feaute
                                        - [ ] If needed i also added the feature to the readme
                                        - [ ] I added the feedback of the ticket creator, that the feature was successful tested
                                        """;


        public async Task GenerateAsync(SolutionFile projectFileInfo,
                                        DirectoryInfo gitHubFolder,
                                        MinimalApiProjectInfos minimalApiProjectInfos)
        {
            // 1. .github folder
            var pullRequestFolder = new DirectoryInfo(Path.Combine(gitHubFolder.FullName, "PULL_REQUEST_TEMPLATE"));
            if (pullRequestFolder.NotExists())
            {
                pullRequestFolder.Create();
            }

            // 2. Write all templates
            var file = Path.Combine(pullRequestFolder.FullName, "pull_request_template.md");

            var newTemplate = Template.Replace("$namespace$", minimalApiProjectInfos.ProjectName)
                                      .Replace("$dotNetToolName$", minimalApiProjectInfos.NormalizedName);


            await File.WriteAllTextAsync(file, newTemplate).ConfigureAwait(false);

            // 3. Print success message
            consoleService.WriteSuccess($"Successfully created {file}");
        }
    }
}
