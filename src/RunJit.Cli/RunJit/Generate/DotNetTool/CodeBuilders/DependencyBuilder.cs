using System.Text;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    public static class AddDependencyBuilderExtension
    {
        public static void AddDependencyBuilder(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<DependencyBuilder>();
        }
    }

    // <summary>
    // What we create here:
    // - Depending on the generated client we will detect all dependencies which are necessary to build up.
    // Sample:
    // - new AdminFacade(new AdminV1(httpDotNetToolHandler)), new AliveFacade(new AliveV1(httpDotNetToolHandler)),
    // </summary>
    internal class DependencyBuilder
    {
        internal string BuildFrom(GeneratedDotNetTool generatedDotNetTool)
        {
            var stringBuilder = new StringBuilder();

            for (var index = 0; index < generatedDotNetTool.Facades.Count; index++)
            {
                // New statement -> new UserFacade(
                var facade = generatedDotNetTool.Facades[index];
                stringBuilder.Append($"new {facade.FacadeName}(");

                // Generate parameter for the facade -> new UserFacade(new UserV1(httpDotNetToolHandler), new UserV2(httpDotNetToolHandler)
                var versionDomains = facade.Endpoints.Select(endpoint => $"new {endpoint.Domain}(httpDotNetToolHandler)").Flatten(", ");
                stringBuilder.Append(versionDomains);

                // If we reach the last parameter we have to close the new statement correctly
                // -> new UserFacade(new UserV1(httpDotNetToolHandler), new UserV2(httpDotNetToolHandler))
                stringBuilder.Append(index < generatedDotNetTool.Facades.Count - 1 ? "), " : ")");
            }

            var result = stringBuilder.ToString();

            return result;
        }
    }
}
