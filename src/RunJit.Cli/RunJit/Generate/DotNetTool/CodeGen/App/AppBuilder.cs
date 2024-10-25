using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    public static class AddAppBuilderCodeGenExtension
    {
        public static void AddAppBuilderCodeGen(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<INetToolCodeGen, AppBuilderCodeGen>();
        }
    }

    internal class AppBuilderCodeGen(IConsoleService consoleService) : INetToolCodeGen
    {
        private const string template = """
                                        using Microsoft.Extensions.DependencyInjection;
                                        
                                        namespace DotNetTool.Builder.App
                                        {
                                            internal sealed class AppBuilder
                                            {
                                                internal App Build()
                                                {
                                                    var services = new ServiceCollection();
                                                    var startup = new Startup();
                                        
                                                    startup.ConfigureServices(services);
                                        
                                                    return new App(services.BuildServiceProvider());
                                                }
                                            }
                                        }
                                        """;

        public async Task GenerateAsync(FileInfo projectFileInfo,
                                        DotNetToolInfos dotNetTool)
        {
            // 1. Add App Folder
            var appFolder = new DirectoryInfo(Path.Combine(projectFileInfo.Directory!.FullName, "App"));
            if (appFolder.NotExists())
            {
                appFolder.Create();
            }

            // 2. Add AppBuilder.cs
            var file = Path.Combine(appFolder.FullName, "AppBuilder.cs");
            await File.WriteAllTextAsync(file, template).ConfigureAwait(false);

            // 3. Print success message
            consoleService.WriteSuccess($"Successfully created {file}");
        }
    }
}
