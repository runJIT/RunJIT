using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    public static class AddExceptionCodeGenExtension
    {
        public static void AddExceptionCodeGen(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<INetToolCodeGen, ExceptionCodeGen>();
        }
    }

    internal class ExceptionCodeGen(IConsoleService consoleService) : INetToolCodeGen
    {
        private const string template = """
                                        using System;

                                        namespace DotNetTool.Builder.ErrorHandling
                                        {
                                            internal sealed class DotNetToolBuilderException(string message) : Exception(message);
                                        }

                                        """;

        public async Task GenerateAsync(FileInfo projectFileInfo,
                                        DotNetToolInfos dotNetTool)
        {
            // 1. Add ErrorHandling Folder
            var appFolder = new DirectoryInfo(Path.Combine(projectFileInfo.Directory!.FullName, "ErrorHandling"));

            if (appFolder.NotExists())
            {
                appFolder.Create();
            }

            // 2. Add .Net too specific exception
            var file = Path.Combine(appFolder.FullName, $"NetToolException.cs");
            await File.WriteAllTextAsync(file, template).ConfigureAwait(false);

            // 3. Add App.csproj
            consoleService.WriteSuccess($"Successfully created {file}");
        }
    }
}
