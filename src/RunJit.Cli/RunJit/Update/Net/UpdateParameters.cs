using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Update.Net
{
    internal static class AddDotNetParametersExtension
    {
        internal static void AddDotNetParameters(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<DotNetParameters>();
        }
    }

    internal record DotNetParameters(string SolutionFile,
                                     int Version);
}
