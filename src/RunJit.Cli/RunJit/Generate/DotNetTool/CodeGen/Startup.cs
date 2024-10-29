﻿using DotNetTool.Service;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;
using Solution.Parser.CSharp;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal interface INetToolCodeGen
    {
        Task GenerateAsync(FileInfo projectFileInfo,
                           DotNetToolInfos dotNetToolInfos);
    }

    internal static class AddStartupCodeGenExtension
    {
        internal static void AddStartupCodeGen(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<INetToolCodeGen, StartupCodeGen>();
        }
    }

    internal sealed class StartupCodeGen(ConsoleService consoleService) : INetToolCodeGen
    {
        private const string Template = """
                                        using $namespace$.$dotNetToolName$;
                                        using Microsoft.Extensions.Configuration;
                                        using Microsoft.Extensions.DependencyInjection;
                                        
                                        namespace $namespace$
                                        {
                                            internal sealed class Startup
                                            {
                                                internal void ConfigureServices(IServiceCollection services, IConfiguration configuration)
                                                {
                                                    // 1. Infrastructure
                                                    services.Add$dotNetToolName$ArgumentFixer();
                                                    services.AddErrorHandler();
                                        
                                                    // 2. Domains
                                                    services.Add$dotNetToolName$CommandBuilder(configuration);
                                                }
                                            }
                                        }
                                        """;

        public async Task GenerateAsync(FileInfo projectFileInfo,
                                        DotNetToolInfos dotNetToolInfos)
        {
            // 1. Add AppBuilder.cs
            var file = Path.Combine(projectFileInfo.Directory!.FullName, "Startup.cs");
            var newTemplate = Template.Replace("$namespace$", dotNetToolInfos.ProjectName)
                                      .Replace("$dotNetToolName$", dotNetToolInfos.DotNetToolName.NormalizedName);

            var formattedTemplate = newTemplate.FormatSyntaxTree();

            await File.WriteAllTextAsync(file, formattedTemplate).ConfigureAwait(false);

            // 2. Print success message
            consoleService.WriteSuccess($"Successfully created {file}");
        }
    }
}
