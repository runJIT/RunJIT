using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;
using Solution.Parser.CSharp;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    public static class AddAppCodeGenExtension
    {
        public static void AddAppCodeGen(this IServiceCollection services)
        {
            services.AddConsoleService();
            services.AddNamespaceProvider();

            services.AddSingletonIfNotExists<INetToolCodeGen, AppCodeGen>();
        }
    }

    internal sealed class AppCodeGen(ConsoleService consoleService,
                              NamespaceProvider namespaceProvider) : INetToolCodeGen
    {
        private const string Template = """
                                        using System.CommandLine;
                                        using System.CommandLine.Builder;
                                        using System.CommandLine.Invocation;
                                        using $namespace$.$dotNetToolName$;
                                        using Extensions.Pack;

                                        namespace $namespace$
                                        {
                                            internal sealed class App(IServiceProvider serviceProvider)
                                            {
                                                internal async Task<int> RunAsync(string[] args)
                                                {
                                                    // 1. Get needed service to invoke client generator. Important anything have to be handled by dependency injection
                                                    var rootCommand = serviceProvider.GetRequiredService<$dotNetToolName$CommandBuilder>().Build();
                                                    var errorHandler = serviceProvider.GetRequiredService<ErrorHandler>();
                                                    var dotNetCliArgumentFixer = serviceProvider.GetRequiredService<$dotNetToolName$ArgumentFixer>();
                                        
                                                    // 2. Setup command line builder from microsoft cli sdk
                                                    var commandLineBuilder = new CommandLineBuilder(rootCommand);
                                                    commandLineBuilder.UseMiddleware(errorHandler.HandleErrorsAsync);
                                                    commandLineBuilder.UseDefaults();
                                                    var parser = commandLineBuilder.Build();
                                        
                                                    // 3. We automatically add a version command
                                                    var option = parser.Configuration.RootCommand.Options.Single(o => o.Name == "version").As<Option?>();
                                                    option?.AddAlias("-v");
                                        
                                                    // 4. Fix or update command parameter
                                                    var fixedArgs = dotNetCliArgumentFixer.Fix(args);
                                        
                                                    // 5. Here the cli sdk of microsoft will be invoked and manage any command execution
                                                    var result = await parser.InvokeAsync(fixedArgs).ConfigureAwait(false);
                                        
                                                    // 6. Return the result of the command execution
                                                    return result;
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

            // 2. Add App.cs
            var file = Path.Combine(appFolder.FullName, "App.cs");

            var newTemplate = Template.Replace("$namespace$", dotNetTool.ProjectName)
                                      .Replace("$dotNetToolName$", dotNetTool.DotNetToolName.NormalizedName);

            var formattedTemplate = newTemplate;

            await File.WriteAllTextAsync(file, formattedTemplate).ConfigureAwait(false);


            // 3. Adjust namespace provider
            namespaceProvider.SetNamespaceProviderAsync(projectFileInfo, $"{dotNetTool.ProjectName}.App", true);

            // 4. Print success message
            consoleService.WriteSuccess($"Successfully created {file}");
        }
    }
}
