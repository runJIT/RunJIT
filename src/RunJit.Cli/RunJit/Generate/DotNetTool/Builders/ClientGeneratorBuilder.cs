using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.ErrorHandling;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal static class AddDotNetToolToolBuildFromStrategyExtension
    {
        internal static void AddDotNetToolToolBuildFromStrategy(this IServiceCollection services)
        {
            services.AddBuildDotNetToolFromConsole();

            services.AddSingletonIfNotExists<DotNetToolGeneratorBuilder>();
        }
    }

    internal class DotNetToolGeneratorBuilder(IEnumerable<IBuildDotNetToolGenerator> dotNetToolStrategies)
    {
        internal DotNetTool BuildFrom(DotNetToolParameters clientGenParameters)
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
