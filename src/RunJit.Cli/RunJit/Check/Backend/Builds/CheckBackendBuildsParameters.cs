using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Check.Backend.Builds
{
    public static class AddCheckBackendBuildsParametersExtension
    {
        public static void AddCheckBackendBuildsParameters(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<CheckBackendBuildsParameters>();
        }
    }

    internal record CheckBackendBuildsParameters(string SolutionFile, string GitRepos, string WorkingDirectory, string IgnorePackages);
}
