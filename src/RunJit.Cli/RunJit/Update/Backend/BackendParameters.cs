using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Update.Backend
{
    public static class AddBackendParametersExtension
    {
        public static void AddBackendParameters(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<BackendParameters>();
        }
    }

    internal record BackendParameters(string Version);
}
