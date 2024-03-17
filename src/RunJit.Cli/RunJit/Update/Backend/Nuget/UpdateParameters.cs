using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Update.Backend.Nuget
{
    public static class AddUpdateNugetParametersExtension
    {
        public static void AddUpdateNugetParameters(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<UpdateNugetParameters>();
        }
    }

    internal record UpdateNugetParameters(string SolutionFile, string GitRepos, string WorkingDirectory, string IgnorePackages);
}
