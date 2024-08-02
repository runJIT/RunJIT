using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Cleanup.Code
{
    public static class AddCleanupCodeParametersExtension
    {
        public static void AddCleanupCodeParameters(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<CleanupCodeParameters>();
        }
    }

    internal record CleanupCodeParameters(string SolutionFile,
                                          string GitRepos,
                                          string WorkingDirectory,
                                          string IgnorePackages);
}
