using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace $namespace$
{
    internal static class GetHealthEndpoint
    {
        internal static void MapGetHealth(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapHealthChecks("/api/health", new HealthCheckOptions()
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            endpointRouteBuilder.MapHealthChecks("/api/v1/health", new HealthCheckOptions()
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
        }
    }
}
