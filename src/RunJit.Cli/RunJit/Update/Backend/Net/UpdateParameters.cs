using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Update.Backend.Net
{
    public static class AddDotNetParametersExtension
    {
        public static void AddDotNetParameters(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<DotNetParameters>();
        }
    }

    internal record DotNetParameters(string SolutionFile, int Version);
}
