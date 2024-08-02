using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.ErrorHandling;

namespace RunJit.Cli.RunJit.Generate.Client
{
    internal static class AddDotNetToolToolBuildFromStrategyExtension
    {
        internal static void AddDotNetToolToolBuildFromStrategy(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<ClientGeneratorBuilder>();
        }
    }

    internal class ClientGeneratorBuilder(IEnumerable<IBuildClientGenerator> dotNetToolStrategies)
    {
        internal Client BuildFrom(ClientParameters clientGenParameters)
        {
            var builder = dotNetToolStrategies.SingleOrDefault(strategy => strategy.IsThisBuilderFor(clientGenParameters));

            if (builder.IsNull())
            {
                throw new RunJitException($"Could not find strategy for your given parameters: {Environment.NewLine}{Environment.NewLine}{clientGenParameters.ToInfo()}");
            }

            return builder.BuildFrom(clientGenParameters);
        }
    }
}
