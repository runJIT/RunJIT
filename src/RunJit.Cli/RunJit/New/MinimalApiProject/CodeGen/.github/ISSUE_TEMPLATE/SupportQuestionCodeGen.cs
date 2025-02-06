using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;
using Solution.Parser.Solution;

namespace RunJit.Cli.New.MinimalApiProject.CodeGen.github.ISSUE_TEMPLATE
{
    internal static class AddSupportQuestionCodeGenExtension
    {
        internal static void AddSupportQuestionCodeGen(this IServiceCollection services)
        {
            services.AddConsoleService();
            services.AddNamespaceProvider();

            services.AddSingletonIfNotExists<IGitHubCodeGen, SupportQuestionCodeGen>();
        }
    }

    internal sealed class SupportQuestionCodeGen(ConsoleService consoleService) : IGitHubCodeGen
    {
        private const string Template = """
                                        ---
                                        name: "\U0001F917 Support Question"
                                        about: "If you have a question \U0001F4AC, please check out our community call!"
                                        title: ''
                                        labels: 'i: question, i: needs triage'
                                        assignees: ''
                                        
                                        ---
                                        
                                        --------------^ Click "Preview" for a nicer view!
                                        We primarily use GitHub as an issue tracker; for usage and support questions, please check out these resources below. Thanks! 😁.
                                        
                                        ---
                                        
                                        * Teams Community Call: join our [teams community call](https://teams.microsoft.com/l/meetup-join/19%3ameeting_ZDdmNTM5YTMtNGQ5ZS00ZmZlLWIzMzktZTE5MzJlYTBjMzJi%40thread.v2/0?context=%7b%22Tid%22%3a%2238ae3bcd-9579-4fd4-adda-b42e1495d55a%22%2c%22Oid%22%3a%22e1e73166-feb0-4509-a5b7-83dd874d70ae%22%7d) once a month.
                                        * Also have a look at the readme for more information on how to get support:
                                          https://github.siemens.cloud/sdc/siemens-data-cloud-core/blob/main/README.md
                                        """;

        public async Task GenerateAsync(SolutionFile projectFileInfo,
                                        DirectoryInfo gitHubFolder,
                                        MinimalApiProjectInfos minimalApiProjectInfos)
        {
            // 1. Issue template folder
            var issueTemplateFolder = new DirectoryInfo(Path.Combine(gitHubFolder.FullName, "ISSUE_TEMPLATE"));
            if (issueTemplateFolder.NotExists())
            {
                issueTemplateFolder.Create();
            }

            // 2. Write all templates
            var file = Path.Combine(issueTemplateFolder.FullName, "Support_question.md");

            var newTemplate = Template.Replace("$namespace$", minimalApiProjectInfos.ProjectName)
                                      .Replace("$dotNetToolName$", minimalApiProjectInfos.NormalizedName);


            await File.WriteAllTextAsync(file, newTemplate).ConfigureAwait(false);

            // 3. Print success message
            consoleService.WriteSuccess($"Successfully created {file}");
        }
    }
}
