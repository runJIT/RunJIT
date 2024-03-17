using System.CommandLine;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.RunJit.Fix.EmbededResources;

namespace RunJit.Cli.RunJit.Fix
{
    public static class AddFixCommandBuilderExtension
    {
        public static void AddFixCommandBuilder(this IServiceCollection services)
        {
            services.AddFixEmbeddedResourcesCommandBuilder();
            
            services.AddSingletonIfNotExists<IRunJitSubCommandBuilder, FixCommandBuilder>();
        }
    }

    internal class FixCommandBuilder(IEnumerable<IFixSubCommandBuilder> subCommandBuilders)
        : IRunJitSubCommandBuilder
    {
        public Command Build()
        {
            var command = new Command("fix", "Ultimate runjit fixtures");
            subCommandBuilders.ForEach(x => command.AddCommand(x.Build()));
            return command;
        }
    }
}
