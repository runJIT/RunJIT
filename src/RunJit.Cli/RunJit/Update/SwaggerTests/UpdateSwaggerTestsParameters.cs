using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Update.SwaggerTests
{
    public static class AddUpdateSwaggerTestsParametersExtension
    {
        public static void AddUpdateSwaggerTestsParameters(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<UpdateSwaggerTestsParameters>();
        }
    }

    internal record UpdateSwaggerTestsParameters(string SolutionFile,
                                                 string GitRepos,
                                                 string WorkingDirectory,
                                                 string IgnorePackages);
}
