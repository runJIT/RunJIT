using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Generate.Client
{
    internal static class AddBuildClientFromConsoleFromConsoleExtension
    {
        internal static void AddBuildClientFromConsole(this IServiceCollection services)
        {
            services.AddClientInfoCollector();

            services.AddSingletonIfNotExists<IBuildClientGenerator, BuildClientFromConsole>();
        }
    }

    internal class BuildClientFromConsole(IClientInfoCollector dotNetToolInfoCollector) : IBuildClientGenerator
    {
        public Client BuildFrom(ClientParameters clientGenParameters)
        {
            return dotNetToolInfoCollector.Collect(clientGenParameters);
        }

        public bool IsThisBuilderFor(ClientParameters clientGenParameters)
        {
            // Currently we have only one strategy
            return true;
        }
    }
}
