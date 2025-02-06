using System.Xml.Linq;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Generate.DotNetTool;
using RunJit.Cli.Services;
using Solution.Parser.CSharp;

namespace RunJit.Cli.New.MinimalApiProject.CodeGen.MinimalApiProject
{
    internal static class AddAppSettingsCodeGenExtension
    {
        internal static void AddAppSettingsCodeGen(this IServiceCollection services)
        {
            services.AddConsoleService();

            services.AddSingletonIfNotExists<IDotNetToolSpecificCodeGen, AppSettingsCodeGen>();
        }
    }

    internal sealed class AppSettingsCodeGen(ConsoleService consoleService) : IDotNetToolSpecificCodeGen
    {
        private const string Template = """
                                        {
                                        }
                                        """;

        public async Task GenerateAsync(FileInfo projectFileInfo,
                                        XDocument projectDocument,
                                        DotNetToolInfos dotNetToolInfos)
        {
            // 1. Add AppSettings.cs
            var file = Path.Combine(projectFileInfo.Directory!.FullName, "appsettings.json");

            var newTemplate = Template.Replace("$clientName$", dotNetToolInfos.ProjectName)
                                      .Replace("$dotNetToolName$", dotNetToolInfos.NormalizedName);

            var formattedTemplate = newTemplate.FormatSyntaxTree();

            await File.WriteAllTextAsync(file, formattedTemplate).ConfigureAwait(false);

            // 4. Print success message
            consoleService.WriteSuccess($"Successfully created {file}");
        }
    }
}
