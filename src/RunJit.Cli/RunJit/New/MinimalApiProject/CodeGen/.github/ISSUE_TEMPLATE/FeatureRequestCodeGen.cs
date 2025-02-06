using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;
using Solution.Parser.Solution;

namespace RunJit.Cli.New.MinimalApiProject.CodeGen.github.ISSUE_TEMPLATE
{
    internal static class AddFeatureRequestCodeGenExtension
    {
        internal static void AddFeatureRequestCodeGen(this IServiceCollection services)
        {
            services.AddConsoleService();
            services.AddNamespaceProvider();

            services.AddSingletonIfNotExists<IGitHubCodeGen, FeatureRequestCodeGen>();
        }
    }

    internal sealed class FeatureRequestCodeGen(ConsoleService consoleService) : IGitHubCodeGen
    {
        private const string Template = """
                                        ---
                                        name: "\U0001F680 Feature Request"
                                        about: "I have a suggestion (and may want to implement it \U0001F642)!"
                                        title: ''
                                        labels: 'i: enhancement, i: needs triage'
                                        assignees: ''
                                        
                                        ---
                                        
                                        ## Feature Request
                                        
                                        **Is your feature request related to a problem? Please describe.**
                                        A clear and concise description of what the problem is. Ex. I have an issue when [...]
                                        
                                        **Describe the solution you'd like**
                                        A clear and concise description of what you want to happen. Add any considered drawbacks.
                                        
                                        **Describe alternatives you've considered**
                                        A clear and concise description of any alternative solutions or features you've considered.
                                        
                                        **Teachability, Documentation, Adoption, Migration Strategy**
                                        If you can, explain how users will be able to use this and possibly write out a version the docs.
                                        Maybe a screenshot or design?
                                        """;


        public async Task GenerateAsync(SolutionFile projectFileInfo,
                                        DirectoryInfo gitHubFolder,
                                        MinimalApiProjectInfos minimalApiProjectInfos)
        {
            // 1. .github folder
            var appFolder = new DirectoryInfo(Path.Combine(gitHubFolder.FullName, "ISSUE_TEMPLATE"));
            if (appFolder.NotExists())
            {
                appFolder.Create();
            }

            // 2. Write all templates
            var file = Path.Combine(appFolder.FullName, "Feature_request.md");

            var newTemplate = Template.Replace("$namespace$", minimalApiProjectInfos.ProjectName)
                                      .Replace("$dotNetToolName$", minimalApiProjectInfos.NormalizedName);


            await File.WriteAllTextAsync(file, newTemplate).ConfigureAwait(false);

            // 3. Print success message
            consoleService.WriteSuccess($"Successfully created {file}");
        }
    }
}
