using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Fix.EmbededResources
{
    internal static class AddFixEmbeddedResourcesArgumentsBuilderExtension
    {
        internal static void AddFixEmbeddedResourcesArgumentsBuilder(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<IFixEmbeddedResourcesArgumentsBuilder, FixEmbeddedResourcesArgumentsBuilder>();
        }
    }

    internal interface IFixEmbeddedResourcesArgumentsBuilder
    {
        IEnumerable<System.CommandLine.Argument> Build();
    }

    internal sealed class FixEmbeddedResourcesArgumentsBuilder : IFixEmbeddedResourcesArgumentsBuilder
    {
        public IEnumerable<System.CommandLine.Argument> Build()
        {
            yield return BuildSourceOption();
        }

        public System.CommandLine.Argument BuildSourceOption()
        {
            return new System.CommandLine.Argument<string>()
                   {
                       Name = "solutionFile",
                   };
        }
    }
}
