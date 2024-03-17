using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Fix.EmbededResources
{
    public static class AddFixEmbeddedResourcesParametersExtension
    {
        public static void AddFixEmbeddedResourcesParameters(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<FixEmbeddedResourcesParameters>();
        }
    }

    internal record FixEmbeddedResourcesParameters(string SolutionFile, string GitRepos, string WorkingDirectory, string IgnorePackages);
}
