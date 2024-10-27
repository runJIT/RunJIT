using System.Collections.Immutable;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.RunJit.Generate.Client;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal static class AddUsingsBuilderExtension
    {
        internal static void AddUsingsBuilder(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<UsingsBuilder>();
        }
    }

    // What we create here:
    // - We create needed using needed from the facades
    // 
    // Samples:
    //
    // using PulseCore.DotNetTool.Api.Admin;
    // using PulseCore.DotNetTool.Api.Alive;
    // using PulseCore.DotNetTool.Api.BulkResources;
    // using PulseCore.DotNetTool.Api.BulkResourceTranslations;
    // using PulseCore.DotNetTool.Api.Caching;
    // using PulseCore.DotNetTool.Api.Category;
    // using PulseCore.DotNetTool.Api.Countries;
    // using PulseCore.DotNetTool.Api.ErrorLog;
    // using PulseCore.DotNetTool.Api.Files;
    // using PulseCore.DotNetTool.Api.Filter;
    internal sealed class UsingsBuilder
    {
        internal string BuildFrom(IImmutableList<GeneratedFacade> facades,
                                  string projectName)
        {
            var usings = facades.Select(facade => $"using {projectName}.{ClientGenConstants.Api}.{facade.Domain};").Flatten(Environment.NewLine);

            return usings;
        }

        internal string BuildFrom(GeneratedDotNetTool generatedDotNetTool,
                                  string projectName)
        {
            var usings = CollectUsings(generatedDotNetTool).Flatten(Environment.NewLine);

            return usings;

            IEnumerable<string> CollectUsings(GeneratedDotNetTool generatedDotNetTool)
            {
                foreach (var facade in generatedDotNetTool.Facades)
                {
                    // Users
                    yield return $"using {projectName}.{ClientGenConstants.Api}.{facade.Domain};";

                    foreach (var endpoint in facade.Endpoints)
                    {
                        // Users.V1
                        yield return $"using {projectName}.{ClientGenConstants.Api}.{facade.Domain}.{endpoint.ControllerInfo.Version.Normalized};";
                    }
                }
            }
        }
    }
}
