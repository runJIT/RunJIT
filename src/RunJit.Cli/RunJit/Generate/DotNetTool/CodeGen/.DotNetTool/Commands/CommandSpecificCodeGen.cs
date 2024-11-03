using System.Xml.Linq;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;

namespace RunJit.Cli.Generate.DotNetTool
{
    internal static class AddCommandCodeGenExtension
    {
        internal static void AddCommandCodeGen(this IServiceCollection services)
        {
            services.AddCommandTypeCollector();
            services.AddNameSpaceCollector();
            services.AddCreateCommandClasses();

            services.AddSingletonIfNotExists<IDotNetToolSpecificCodeGen, CommandCodeGen>();
        }
    }

    internal sealed class CommandCodeGen(ConsoleService consoleService,
                                         CommandTypeCollector commandTypeCollector,
                                         NameSpaceCollector nameSpaceCollector,
                                         CreateCommandClasses createCommandClasses) : IDotNetToolSpecificCodeGen
    {
        public Task GenerateAsync(FileInfo projectFileInfo,
                                  XDocument projectDocument,
                                  DotNetToolInfos dotNetToolInfos)
        {
            createCommandClasses.Invoke(dotNetToolInfos.ProjectName, dotNetToolInfos.CommandInfo, projectFileInfo.Directory!,
                                        commandTypeCollector, dotNetToolInfos.ProjectName, nameSpaceCollector,
                                        dotNetToolInfos);

            // 2. Print success message
            consoleService.WriteSuccess($"Successfully created cli structure");

            return Task.CompletedTask;
        }
    }
}
