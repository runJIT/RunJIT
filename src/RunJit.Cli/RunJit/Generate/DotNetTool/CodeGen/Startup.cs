using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal interface INetToolCodeGen
    {
        Task GenerateAsync(FileInfo projectFileInfo,
                           DotNetToolInfos dotNetToolInfos);
    }

    public static class AddStartupCodeGenExtension
    {
        public static void AddStartupCodeGen(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<INetToolCodeGen, StartupCodeGen>();
        }
    }
    
    internal class StartupCodeGen(IConsoleService consoleService) : INetToolCodeGen
    {
        private const string template = """
                                        using DotNetTool.Service;
                                        using Extensions.Pack;
                                        using Microsoft.Extensions.Configuration;
                                        using Microsoft.Extensions.DependencyInjection;
                                        using RunJit.Cli.Auth0;
                                        using RunJit.Cli.ErrorHandling;
                                        using RunJit.Cli.RunJit;
                                        
                                        namespace RunJit.Cli
                                        {
                                            internal class Startup
                                            {
                                                internal void ConfigureServices(IServiceCollection services,
                                                                                IConfiguration configuration)
                                                {
                                                    // 1. Infrastructure
                                                    services.AddDotNetCliArgumentFixer();
                                                    services.AddErrorHandler();
                                        
                                                    // 2. Domains
                                                    services.AddRunJitCommandBuilder(configuration);
                                        
                                                    // 3. External tools
                                                    var dotnetTool = DotNetToolFactory.Create();
                                                    services.AddSingletonIfNotExists(dotnetTool);
                                                }
                                            }
                                        }
                                        """;

        public async Task GenerateAsync(FileInfo projectFileInfo,
                                        DotNetToolInfos dotNetToolInfos)
        {
            // 1. Add AppBuilder.cs
            var file = Path.Combine(projectFileInfo.Directory!.FullName, "Startup.cs");
            await File.WriteAllTextAsync(file, template).ConfigureAwait(false);

            // 2. Print success message
            consoleService.WriteSuccess($"Successfully created {file}");
        }
    }
}
