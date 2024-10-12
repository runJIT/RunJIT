using System.Collections.Immutable;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;
using Solution.Parser.CSharp;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal static class AddQueryBuilderExtension
    {
        internal static void AddQueryBuilder(this IServiceCollection services)
        {
            services.AddEnumerationTypes();

            services.AddSingletonIfNotExists<QueryBuilder>();
        }
    }

    // What we are create here:
    // - We create the needed call url for the implemented http method / controller method.
    //
    // Samples:
    // 
    // ?useCache={useCache}
    // ?useCache={useCache}&withSqlAnalytics={withSqlAnalytics}&parentId={parentId}
    // ?{resourceTypeIds.ToQueryParams("resourceTypeId")}&useCache={useCache}&offset={offset}&limit={limit}&search={search}
    internal class QueryBuilder(IEnumerationTypes enumerationTypes)
    {
        private const string FromQuery = "FromQuery";

        internal string BuildFrom(Method method)
        {
            var queryParams = CollectQueryParam(method).ToImmutableList();
            if (queryParams.IsEmpty())
            {
                return string.Empty;
            }

            var queryParamsFlatten = queryParams.Flatten("&");
            return $"?{queryParamsFlatten}";
        }

        private IEnumerable<string> CollectQueryParam(Method method)
        {
            foreach (var parameter in method.Parameters)
            {
                // we only interested in from query params
                if (parameter.Attributes.All(a => a.Name.NotEqualsTo(FromQuery)))
                {
                    continue;
                }

                var argument = parameter.Attributes.FirstOrDefault(attribute => attribute.Name.EqualsTo(FromQuery))?.Arguments.FirstOrDefault(a => a.Contains("Name ="))?.Split('=')?.Last()?.Trim().Trim('"');
                var parameterName = argument ?? parameter.Name;

                if (enumerationTypes.IsListType(parameter.Type))
                {
                    // {resourceTypeIds.ToQueryParams("resourceTypeId")}
                    yield return @$"{{{parameter.Name}.ToQueryParams(""{parameterName}"")}}";
                    continue;
                }

                // useCache={useCache}
                yield return $"{parameterName}={{{parameter.Name}}}";
            }
        }
    }
}
