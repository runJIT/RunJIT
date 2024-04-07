using System.Collections.Immutable;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Generate.Client
{
    internal static class AddAssignExpressionBuilderExtension
    {
        internal static void AddAssignExpressionBuilder(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<AssignExpressionBuilder>();
        }
    }

    // What we are build here:
    // - The assignment expression for properties or fields
    // Sample:
    // 
    // Admin = adminFacade;
    // Alive = aliveFacade;
    // BulkResources = bulkResourcesFacade;
    internal class AssignExpressionBuilder
    {
        internal string BuildFrom(IImmutableList<GeneratedFacade> facades)
        {
            var assignments = facades.Select(f => $"\t\t\t{f.Domain}".FirstCharToUpper() + " = " + $"{f.FacadeName};".FirstCharToLower())
                                     .Flatten(Environment.NewLine);

            return assignments;
        }

        internal string BuildFrom(IGrouping<string, GeneratedClientCodeForController> groupedEndpoints)
        {
            var assignments = groupedEndpoints.Select(f => $"\t\t\t{f.ControllerInfo.Version.Normalized}".ToUpperInvariant() + " = " + $"{f.Domain};".FirstCharToLower())
                                              .Flatten(Environment.NewLine);

            return assignments;
        }
    }
}
