using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;

namespace RunJit.Cli.Generate.DotNetTool
{
    internal static class AddDotNetToolExtension
    {
        internal static void AddDotNetTool(this IServiceCollection services)
        {
            services.AddConsoleService();
            services.AddDotNetToolCreator();
            services.AddProcessService();

            services.AddSingletonIfNotExists<IDotNetToolGen, DotNetToolGen>();
        }
    }

    internal interface IDotNetToolGen
    {
        Task<int> HandleAsync(DotNetToolParameters parameters);
    }

    internal sealed class DotNetToolGen(IProcessService processService,
                                        ConsoleService consoleService,
                                        DotNetToolCreator dotNetToolCreator) : IDotNetToolGen
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
            var projectName = $"{parameters.SolutionFile.NameWithoutExtension()}.DotNetTool";

            // 2. Generate the dotnet tool into solution
            await dotNetToolCreator.GenerateDotNetToolAsync(projectName, $"dotnet-{parameters.ToolName}", parameters.ToolName,
                                                            parameters.SolutionFile).ConfigureAwait(false);

            // 3. Write success message
            consoleService.WriteSuccess($"Enjoy your new generated: '{projectName}' .net tool");

            return 0;
        }
    }
}
