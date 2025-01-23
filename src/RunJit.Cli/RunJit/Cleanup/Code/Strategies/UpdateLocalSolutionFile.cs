using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.ErrorHandling;
using RunJit.Cli.Services;
using RunJit.Cli.Services.Net;

namespace RunJit.Cli.RunJit.Cleanup.Code
{
    internal static class AddUpdateLocalSolutionFileExtension
    {
        internal static void AddUpdateLocalSolutionFile(this IServiceCollection services)
        {
            // services.AddCleanupCodePackageService();
            services.AddFindSolutionFile();
            services.AddDotNet();

            services.AddSingletonIfNotExists<ICleanupCodeStrategy, UpdateLocalSolutionFile>();
        }
    }

    internal sealed class UpdateLocalSolutionFile(FindSolutionFile findSolutionFile,
                                                  IDotNet dotNet) : ICleanupCodeStrategy
    {
        public bool CanHandle(CleanupCodeParameters parameters)
        {
            return parameters.SolutionFile.IsNotNullOrWhiteSpace();
        }

        public async Task HandleAsync(CleanupCodeParameters parameters)
        {
            // 0. Check that precondition is met
            if (CanHandle(parameters).IsFalse())
            {
                throw new RunJitException($"Please call {nameof(ICleanupCodeStrategy.CanHandle)} before call {nameof(ICleanupCodeStrategy.HandleAsync)}");
            }

            var solutionFile = findSolutionFile.Find(parameters.SolutionFile);

            // 7. Check for R# dot settings if existed, if not create one
            var dotSettingsFile = new FileInfo($"{solutionFile.FullName}.DotSettings");

            if (dotSettingsFile.NotExists())
            {
                var fileContent = EmbeddedFile.GetFileContentFrom("Resharper.sln.DotSettings");
                await File.WriteAllTextAsync(dotSettingsFile.FullName, fileContent).ConfigureAwait(false);
            }

            // 8. Build the solution first, we can not clean up the code if the solution is not building
            await dotNet.BuildAsync(solutionFile).ConfigureAwait(false);
        }
    }
}
