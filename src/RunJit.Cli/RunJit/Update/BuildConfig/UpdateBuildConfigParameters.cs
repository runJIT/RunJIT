using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Update.BuildConfig
{
    public static class AddUpdateBuildConfigParametersExtension
    {
        public static void AddUpdateBuildConfigParameters(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<UpdateBuildConfigParameters>();
        }
    }
    
    internal record UpdateBuildConfigParameters(string SolutionFile,
                                                string GitRepos,
                                                string WorkingDirectory);
}
