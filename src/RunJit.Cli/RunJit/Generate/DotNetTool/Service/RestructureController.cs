using System.Collections.Immutable;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services.Endpoints;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    public static class AddRestructureControllerExtension
    {
        public static void AddRestructureController(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<RestructureController>();
        }
    }

    internal class RestructureController
    {
        internal IImmutableList<ControllerInfo> Reorganize(ImmutableList<ControllerInfo> controllers)
        {
            var reorganizedControllers = ReorganizeInternal(controllers).ToImmutableList();

            return reorganizedControllers;

            static IEnumerable<ControllerInfo> ReorganizeInternal(ImmutableList<ControllerInfo> controllers)
            {
                var groupedByVersions = controllers.GroupBy(controller => controller.Version).ToImmutableList();

                foreach (var groupByVersion in groupedByVersions)
                {
                    var groupedByDomain = groupByVersion.GroupBy(controller => controller.GroupName).ToImmutableList();

                    foreach (var boundContext in groupedByDomain)
                    {
                        var methods = boundContext.SelectMany(item => item.Methods).ToImmutableList();
                        var attributes = boundContext.SelectMany(item => item.Attributes).DistinctBy(b => b.Name).ToImmutableList();
                        var normalizedGroupName = boundContext.Key.Where(char.IsLetterOrDigit).ToFlattenString();

                        yield return new ControllerInfo
                                     {
                                         DomainName = normalizedGroupName,
                                         GroupName = normalizedGroupName,
                                         Name = normalizedGroupName,
                                         Methods = methods,
                                         BaseUrl = boundContext.First().BaseUrl,
                                         Version = groupByVersion.Key,
                                         Attributes = attributes
                                     };
                    }
                }
            }
        }
    }
}
