using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal static class AddDotNetToolExtension
    {
        internal static void AddDotNetTool(this IServiceCollection services)
        {
            services.AddConsoleService();
            services.AddDotNetToolToolBuildFromStrategy();
            services.AddDotNetToolCreator();
            services.AddProcessService();
            services.AddTargetFolderService();
            services.AddTemplateExtractor();
            services.AddTemplateService();

            services.AddSingletonIfNotExists<IDotNetToolGen, DotNetToolGen>();
        }
    }

    internal interface IDotNetToolGen
    {
        Task<int> HandleAsync(DotNetToolParameters parameters);
    }

    internal class DotNetToolGen(IProcessService processService,
                                 IConsoleService consoleService,
                                 DotNetToolCreator dotNetToolCreator,
                                 DotNetToolGeneratorBuilder clientGeneratorBuilder) : IDotNetToolGen
    {
        public async Task<int> HandleAsync(DotNetToolParameters parameters)
        {
            // ToDo: Idea a new parameter to control with or without build :)
            // 0. Build the target solution first
            if (parameters.Build)
            {
                var dotnetBuildResult = await processService.RunAsync("dotnet", $"build {parameters.SolutionFile.FullName}").ConfigureAwait(false);
                if (dotnetBuildResult.ExitCode != 0)
                {
                    return dotnetBuildResult.ExitCode;
                }
            }

            // 1. Build client generator from parameters
            var client = clientGeneratorBuilder.BuildFrom(parameters);

            //// 7. Generate client code
            await dotNetToolCreator.GenerateDotNetToolAsync(client, parameters.SolutionFile).ConfigureAwait(false);

            consoleService.WriteSuccess($"Enjoy your new generated: '{client.ProjectName}' .net tool");
            return 0;
        }
    }
}
