﻿using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
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
                                        using Microsoft.Extensions.Configuration.Json;
                                        
                                        namespace $namespace$
                                        {
                                            internal sealed class AppBuilder
                                            {
                                                internal App Build()
                                                {
                                                    // 1. Setup startup
                                                    var startup = new Startup();
                                        
                                                    // 2. Setup service collection
                                                    var services = new ServiceCollection();
                                        
                                                    // 3. Setup configuration builder
                                                    var configurationBuilder = new ConfigurationBuilder();
                                        
                                                    // 3.1 We are using embedded appsettings cause of deployments of .net tool.
                                                    var appsettingsAsStream = typeof(AppBuilder).Assembly.GetEmbeddedFileAsStream("appsettings.json");
                                                    var jsonStreamConfigurationSource = new JsonStreamConfigurationSource
                                                    {
                                                        Stream = appsettingsAsStream
                                                    };
                                        
                                                    // 3.2 Add json stream configuration source
                                                    configurationBuilder.Add(jsonStreamConfigurationSource);
                                        
                                                    // 3.3 Add environment variables. With this we can overwrite all used settings :)
                                                    configurationBuilder.AddEnvironmentVariables();
                                        
                                                    // 3.4 Add user secrets. With this we can overwrite all used settings :)
                                                    configurationBuilder.AddUserSecrets(typeof(AppBuilder).Assembly);
                                        
                                                    // 4. Build configuration
                                                    var configuration = configurationBuilder.Build();
                                        
                                                    // 5. Call startup to configure and setup the .net tool
                                                    startup.ConfigureServices(services, configuration);
                                        
                                                    // 6. Build service provider
                                                    var buildServiceProvider = services.BuildServiceProvider();
                                        
                                                    // 7. Provide the ready to use app
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

            var formattedTemplate = newTemplate;

            await File.WriteAllTextAsync(file, formattedTemplate).ConfigureAwait(false);

            // 3. Print success message
            consoleService.WriteSuccess($"Successfully created {file}");
        }
    }
}
