using System.Collections.Immutable;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Extensions;
using Solution.Parser.AspNet;
using Solution.Parser.CSharp;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal static class AddApiVersionFinderExtension
    {
        internal static void AddApiVersionFinder(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<ApiVersionFinder>();
        }
    }

    public class ApiVersionFinder
    {
        public IImmutableList<ApiVersion> FindAllApiVersions(IImmutableList<CSharpSyntaxTree> syntaxTrees)
        {
            var controllers = (from syntaxTree in syntaxTrees
                               from @class in syntaxTree.Classes
                               where @class.BaseTypes.Any(baseType => baseType.TypeName.Contains("Controller")) // ODataController, Controller, ControllerBase
                               select @class).ToImmutableList();

            var allVersions = controllers.SelectMany(controller => controller.Attributes.Where(a => a.Name == "ApiVersion"))
                                         .Select(a => a.Arguments.FirstOrDefault()?.Replace("\"", string.Empty) ?? string.Empty)
                                         .Distinct()
                                         .Select(version => version.ToApiVersion());

            var orderedVersions = allVersions.OrderBy(v => v.Major).ThenBy(v => v.Minor).ToImmutableList();

            return orderedVersions;
        }
    }
}
