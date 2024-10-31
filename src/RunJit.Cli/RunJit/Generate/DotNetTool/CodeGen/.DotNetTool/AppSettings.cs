using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;
using Solution.Parser.CSharp;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
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
                                          "$dotNetToolName$HttpClientSettings": {
                                            "BaseAddress": "https://localhost:7221/"
                                          }
                                        }
                                        """;

        public async Task GenerateAsync(FileInfo projectFileInfo,
                                        DotNetToolInfos dotNetTool)
        {

            // 1. Add AppSettings.cs
            var file = Path.Combine(projectFileInfo.Directory!.FullName, "appsettings.json");

            var newTemplate = Template.Replace("$clientName$", dotNetTool.ProjectName)
                                      .Replace("$dotNetToolName$", dotNetTool.NormalizedName);

            var formattedTemplate = newTemplate.FormatSyntaxTree();

            await File.WriteAllTextAsync(file, formattedTemplate).ConfigureAwait(false);

            // 4. Print success message
            consoleService.WriteSuccess($"Successfully created {file}");
        }
    }
}
