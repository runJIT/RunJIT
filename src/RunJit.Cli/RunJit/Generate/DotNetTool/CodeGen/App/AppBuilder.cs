using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.RunJit.Generate.CustomEndpoint;
using RunJit.Cli.Services;
using Solution.Parser.CSharp;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    public static class AddAppBuilderCodeGenExtension
    {
        public static void AddAppBuilderCodeGen(this IServiceCollection services)
        {
            services.AddConsoleService();

            services.AddSingletonIfNotExists<INetToolCodeGen, AppBuilderCodeGen>();
        }
    }

    internal sealed class AppBuilderCodeGen(ConsoleService consoleService) : INetToolCodeGen
    {
        private const string Template = """
                                        using Extensions.Pack;
                                        using Microsoft.Extensions.Configuration;
                                        using Microsoft.Extensions.Configuration.Json;
                                        using Microsoft.Extensions.DependencyInjection;

                                        namespace $namespace$
                                        {
                                            internal sealed class AppBuilder
                                            {
                                                internal App Build()
                                                {
                                                    var startup = new Startup();
                                                    var services = new ServiceCollection();
                                                    
                                                    var configurationBuilder = new ConfigurationBuilder();
                                                    var appsettingsAsStream = typeof(AppBuilder).Assembly.GetEmbeddedFileAsStream("appsettings.json");
                                                    var jsonStreamConfigurationSource = new JsonStreamConfigurationSource { Stream = appsettingsAsStream };
                                                    
                                                    configurationBuilder.Add(jsonStreamConfigurationSource);
                                                    configurationBuilder.AddEnvironmentVariables();
                                                    configurationBuilder.AddUserSecrets(typeof(AppBuilder).Assembly);
                                                    var configuration = configurationBuilder.Build();
                                                    
                                                    startup.ConfigureServices(services, configuration);
                                            
                                                    var buildServiceProvider = services.BuildServiceProvider();
                                            
                                                    return new App(buildServiceProvider);
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

            var newTemplate = Template.Replace("$namespace$", dotNetTool.ProjectName)
                                      .Replace("$dotNetToolName$", dotNetTool.DotNetToolName.NormalizedName);

            var formattedTemplate = newTemplate.FormatSyntaxTree();

            await File.WriteAllTextAsync(file, formattedTemplate).ConfigureAwait(false);

            // 3. Print success message
            consoleService.WriteSuccess($"Successfully created {file}");
        }
    }
}
