using System.Text.RegularExpressions;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.RunJit.Update.BuildConfig;

namespace RunJit.Cli.RunJit.Update.UpdateBuildConfig
{
    public static class AddUpdateBuildConfigServiceExtension
    {
        public static void AddUpdateBuildConfigService(this IServiceCollection services)
        {
            services.AddConsoleService();
            services.AddUpdateBuildConfigParameters();
            services.AddFindSolutionFile();

            services.AddSingletonIfNotExists<IUpdateBuildConfigService, UpdateBuildConfigService>();
        }
    }

    internal interface IUpdateBuildConfigService
    {
        Task HandleAsync(UpdateBuildConfigParameters parameters);
    }

    internal class UpdateBuildConfigService(IConsoleService consoleService,
                                      FindSolutionFile findSolutionFile) : IUpdateBuildConfigService
    {
        public async Task HandleAsync(UpdateBuildConfigParameters parameters)
        {
            // 1. Check if solution file is the file or directory
            //    if it is null or whitespace we check current directory
            var solutionFile = findSolutionFile.Find(parameters.SolutionFile);

            // 2. Directory.Build.props
            var file = Path.Combine(solutionFile.Directory!.FullName, "Directory.Build.props");
            
            // 3. FileContent
            var content = EmbeddedFile.GetFileContentFrom("RunJit.Update.BuildConfig.Templates.Directory.Build.props");

            // 4. Write directory.build.props
            await File.WriteAllTextAsync(file, content).ConfigureAwait(false);

            consoleService.WriteSuccess($"Directory.Build.props was successful updated");
        }
    }
}
