using MinimalApi.Api.Health.V1.GetHealthState;

namespace MinimalApi.Api.Health.V1
{
    internal static class Startup
    {
        internal static void AddHealthV1(this IServiceCollection services)
        {
            services.AddGetHealthState();
        }

        internal static void MapHealthV1(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapGetHealthState();
        }
    }
}
