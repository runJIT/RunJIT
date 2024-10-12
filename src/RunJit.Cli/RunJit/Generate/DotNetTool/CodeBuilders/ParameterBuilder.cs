using System.Collections.Immutable;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal static class AddParamaterBuilderExtension
    {
        internal static void AddParamaterBuilder(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<ParameterBuilder>();
        }
    }

    // What we are create here:
    // - We create the parameters for dependency injection
    //
    // Sample:
    // 
    // AdminFacade adminFacade, AliveFacade aliveFacade
    internal class ParameterBuilder
    {
        internal string BuildFrom(IImmutableList<GeneratedFacade> facades)
        {
            var parameters = facades.Select(f => $"{f.FacadeName} " + $"{f.FacadeName}".FirstCharToLower())
                                    .Flatten(", ");

            return parameters;
        }

        internal string BuildFrom(IGrouping<string, GeneratedDotNetToolCodeForController> groupedEndpoints)
        {
            var parameters = groupedEndpoints.Select(f => $"{f.Domain} " + $"{f.Domain}".FirstCharToLower())
                                             .Flatten(", ");

            return parameters;
        }
    }
}
