using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal static class AddBuildDotNetToolFromConsoleFromConsoleExtension
    {
        internal static void AddBuildDotNetToolFromConsole(this IServiceCollection services)
        {
            services.AddDotNetToolInfoCollector();

            services.AddSingletonIfNotExists<IBuildDotNetToolGenerator, BuildDotNetToolFromConsole>();
        }
    }

    internal class BuildDotNetToolFromConsole(IDotNetToolInfoCollector dotNetToolInfoCollector) : IBuildDotNetToolGenerator
    {
        public DotNetTool BuildFrom(DotNetToolParameters clientGenParameters)
        {
            return dotNetToolInfoCollector.Collect(clientGenParameters);
        }

        public bool IsThisBuilderFor(DotNetToolParameters clientGenParameters)
        {
            // Currently we have only one strategy
            return true;
        }
    }
}
