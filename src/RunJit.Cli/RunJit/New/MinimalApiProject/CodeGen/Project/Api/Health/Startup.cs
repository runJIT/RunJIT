using MinimalApi.Api.Health.V1;

namespace MinimalApi.Api.Health
{
    internal static class Startup
    {
        internal static void AddHealth(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddHealthV1();
        }

        internal static void MapHealth(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapHealthV1();
        }
    }
}
