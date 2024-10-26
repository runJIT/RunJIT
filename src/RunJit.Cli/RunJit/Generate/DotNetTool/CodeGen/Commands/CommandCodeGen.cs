using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    public static class AddCommandCodeGenExtension
    {
        public static void AddCommandCodeGen(this IServiceCollection services)
        {
            services.AddCommandTypeCollector();
            services.AddNameSpaceCollector();
            services.AddCreateCommandClasses();

            services.AddSingletonIfNotExists<INetToolCodeGen, CommandCodeGen>();
        }
    }

    internal class CommandCodeGen(ConsoleService consoleService,
                                  CommandTypeCollector commandTypeCollector,
                                  NameSpaceCollector nameSpaceCollector,
                                  CreateCommandClasses createCommandClasses) : INetToolCodeGen
    {
        public Task GenerateAsync(FileInfo projectFileInfo,
                                  DotNetToolInfos dotNetToolInfos)
        {
            createCommandClasses.Invoke(dotNetToolInfos.ProjectName, dotNetToolInfos.CommandInfo, projectFileInfo.Directory!,
                                        commandTypeCollector, dotNetToolInfos.ProjectName, nameSpaceCollector);

            // 2. Print success message
            consoleService.WriteSuccess($"Successfully created cli structure");

            return Task.CompletedTask;
        }
    }
}
