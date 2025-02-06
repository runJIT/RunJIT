using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.New.MinimalApiProject
{
    internal static class AddNewMinimalApiProjectParametersExtension
    {
        internal static void AddNewMinimalApiProjectParameters(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<NewMinimalApiProjectParameters>();
        }
    }

    internal sealed record NewMinimalApiProjectParameters(bool UseVisualStudio,
                                                          bool Build,
                                                          string ProjectName,
                                                          string BasePath,
                                                          DirectoryInfo TargetDirectoryInfo,
                                                          int TargetFramework);
}
