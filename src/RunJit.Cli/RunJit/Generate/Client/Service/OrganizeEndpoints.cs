using System.Collections.Immutable;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services.Endpoints;

namespace RunJit.Cli.RunJit.Generate.Client
{
    internal static class AddOrganizeMinimalEndpointsExtension
    {
        internal static void AddOrganizeMinimalEndpoints(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<OrganizeMinimalEndpoints>();
        }
    }

    internal sealed class OrganizeMinimalEndpoints
    {
        internal ImmutableList<EndpointGroup> Reorganize(ImmutableList<EndpointInfo> endpointInfos)
        {
            var reorganizedControllers = ReorganizeInternal(endpointInfos).ToImmutableList();

            return reorganizedControllers;

            static IEnumerable<EndpointGroup> ReorganizeInternal(ImmutableList<EndpointInfo> endpointInfos)
            {
                var groupedByVersions = endpointInfos.GroupBy(controller => controller.Version).ToImmutableList();

                foreach (var groupByVersion in groupedByVersions)
                {
                    var groupedByDomain = groupByVersion.GroupBy(controller => controller.GroupName).ToImmutableList();

                    foreach (var boundContext in groupedByDomain)
                    {
                        var normalizedGroupName = string.Join("", boundContext.Key.Where(char.IsLetterOrDigit));

                        yield return new EndpointGroup
                                     {
                                         GroupName = normalizedGroupName,
                                         Endpoints = boundContext.ToImmutableList(),
                                         Version = groupByVersion.Key
                                     };
                    }
                }
            }
        }
    }
}
