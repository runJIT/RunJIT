using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.Resharper
{
    public static class AddSolutionCodeCleanupExtension
    {
        public static void AddSolutionCodeCleanup(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<SolutionCodeCleanup>();
        }
    }

    internal class SolutionCodeCleanup(DotNetTool.Service.DotNetTool dotnetTool,
                                       IConsoleService consoleService)
    {
        private const string ResharperToolName = "JetBrains.ReSharper.GlobalTools";

        internal async Task CleanupAsync(FileInfo solutionFile)
        {
            // 1. Check that R# dotnet tool is installed
            consoleService.WriteInfo($"Check if {ResharperToolName} are already installed");
            var resharperTool = await dotnetTool.ExistsAsync(ResharperToolName).ConfigureAwait(false);
            if (resharperTool.IsNull())
            {
                consoleService.WriteInfo($"Try to install {ResharperToolName} for code inspection and cleanups");
                var installResult = await dotnetTool.InstallAsync(ResharperToolName).ConfigureAwait(false);

                if (installResult.ExitCode == 0)
                {
                    consoleService.WriteSuccess($"Installation of the {ResharperToolName} was successful.{Environment.NewLine}{installResult.Output}");
                }
                else
                {
                    consoleService.WriteError($"Installation of the {ResharperToolName} failed.{Environment.NewLine}{installResult.Output}");
                }
            }
            else
            {
                consoleService.WriteInfo($"Try to update {ResharperToolName} for code inspection and cleanups");
                var updateResult = await dotnetTool.UpdateAsync(ResharperToolName).ConfigureAwait(false);
                if (updateResult.ExitCode == 0)
                {
                    consoleService.WriteSuccess($"Installation of the {ResharperToolName} was successful.{Environment.NewLine}{updateResult.Output}");
                }
                else
                {
                    consoleService.WriteError($"Installation of the {ResharperToolName} failed.{Environment.NewLine}{updateResult.Output}");
                }
            }

            // 2. Run R# code cleanup
            consoleService.WriteInfo($"Start code cleanup for solution: {solutionFile.FullName}");
            var cleanupResult = await dotnetTool.RunAsync("jd", $"cleanupcode {solutionFile.FullName}").ConfigureAwait(false);

            if (cleanupResult.ExitCode == 0)
            {
                consoleService.WriteSuccess($"Code cleanup in solution {solutionFile.FullName} was successful");
            }
            else
            {
                consoleService.WriteError($"Code cleanup in solution {solutionFile.FullName} failed");
            }
        }
    }
}
