using Extensions.Pack;
using Microsoft.OpenApi.Models;

namespace $ProjectName$.OpenApi
{
    internal static class AddOpenApiInfosExtension
    {
        internal static void AddOpenApiInfos(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.TryGetSettings<OpenApiInfos>(out var openApiInfos).IsFalse())
            {
                openApiInfos = new OpenApiInfos();
            }
            services.AddSingletonIfNotExists(openApiInfos);
        }
    }

    internal sealed record OpenApiInfos
    {
        public OpenApiVersionInfo[] Versions { get; init; } = [];

    }
}
