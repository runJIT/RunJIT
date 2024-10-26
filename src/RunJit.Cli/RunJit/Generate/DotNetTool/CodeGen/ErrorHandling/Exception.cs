using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;
using Solution.Parser.CSharp;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    public static class AddExceptionCodeGenExtension
    {
        public static void AddExceptionCodeGen(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<INetToolCodeGen, ExceptionCodeGen>();
        }
    }

    internal class ExceptionCodeGen(ConsoleService consoleService,
                                    NamespaceProvider namespaceProvider) : INetToolCodeGen



    {
        private const string Template = """
                                        namespace $namespace$
                                        {
                                            internal sealed class $dotNetToolName$Exception(string message) : Exception(message);
                                        }

                                        """;

        public async Task GenerateAsync(FileInfo projectFileInfo,
                                        DotNetToolInfos dotNetToolInfos)
        {
            // 1. Add ErrorHandling Folder
            var appFolder = new DirectoryInfo(Path.Combine(projectFileInfo.Directory!.FullName, "ErrorHandling"));

            if (appFolder.NotExists())
            {
                appFolder.Create();
            }

            // 2. Add .Net too specific exception
            var file = Path.Combine(appFolder.FullName, $"{dotNetToolInfos.DotNetToolName.NormalizedName}Exception.cs");

            var newTemplate = Template.Replace("$namespace$", dotNetToolInfos.ProjectName)
                                      .Replace("$dotNetToolName$", dotNetToolInfos.DotNetToolName.NormalizedName);

            var formattedTemplate = newTemplate.FormatSyntaxTree();

            await File.WriteAllTextAsync(file, formattedTemplate).ConfigureAwait(false);

            // 3. Adjust namespace provider
            namespaceProvider.SetNamespaceProviderAsync(projectFileInfo, $"{dotNetToolInfos.ProjectName}.ErrorHandling", true);

            // 4. Add App.csproj
            consoleService.WriteSuccess($"Successfully created {file}");
        }
    }
}
