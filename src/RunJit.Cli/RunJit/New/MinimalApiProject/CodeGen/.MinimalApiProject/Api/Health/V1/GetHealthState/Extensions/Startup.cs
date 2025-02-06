using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MinimalApi.Api.Health.V1.GetHealthState
{
    internal static class Startup
    {
        internal static void AddGetHealthState(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddHealthChecks();
        }

        internal static void MapGetHealthState(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapGetHealth();
        }
    }
}
