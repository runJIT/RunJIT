using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    public static class AddAppCodeGenExtension
    {
        public static void AddAppCodeGen(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<INetToolCodeGen, AppCodeGen>();
        }
    }

    internal class AppCodeGen(IConsoleService consoleService) : INetToolCodeGen
    {
        private const string template = """
                                        using System;
                                        using System.CommandLine;
                                        using System.CommandLine.Builder;
                                        using System.CommandLine.Invocation;
                                        using System.Linq;
                                        using System.Threading.Tasks;
                                        using Argument.Check;
                                        using DotNetTool.Builder.DotNet;
                                        using DotNetTool.Builder.ErrorHandling;
                                        using DotNetTool.Builder.Services.DotNet;
                                        using FileSystem.Abstraction;
                                        using Microsoft.Extensions.DependencyInjection;
                                        
                                        namespace DotNetTool.Builder.App
                                        {
                                            internal sealed class App(IServiceProvider serviceProvider)
                                            {
                                                public Task<int> RunAsync(string[] args)
                                                {
                                                    Throw.IfNull(() => args);
                                        
                                                    var rootCommand = serviceProvider.GetService<IDotnetCommandBuilder>().Build();
                                                    var errorHandler = serviceProvider.GetService<IErrorHandler>();
                                                    var dotNetCliArgumentFixer = serviceProvider.GetService<IDotNetCliArgumentFixer>();
                                        
                                                    var directoryService = serviceProvider.GetService<IDirectoryService>();
                                                    var currentDirectory = directoryService.GetDirectoryInfo(Environment.CurrentDirectory);
                                                    directoryService.SetCurrentDirectoryInfo(currentDirectory);
                                        
                                                    var commandLineBuilder = new CommandLineBuilder(rootCommand);
                                                    commandLineBuilder.UseMiddleware(errorHandler.HandleErrors);
                                                    commandLineBuilder.UseDefaults();
                                                    var parser = commandLineBuilder.Build();
                                        
                                                    var option = parser.Configuration.RootCommand.Options.Single(o => o.Name == "version") as Option;
                                                    option?.AddAlias("-v");
                                        
                                                    var fixedArgs = dotNetCliArgumentFixer.Fix(args);
                                                    return parser.InvokeAsync(fixedArgs);
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
            await File.WriteAllTextAsync(file, template).ConfigureAwait(false);

            // 3. Print success message
            consoleService.WriteSuccess($"Successfully created {file}");
        }
    }
}
