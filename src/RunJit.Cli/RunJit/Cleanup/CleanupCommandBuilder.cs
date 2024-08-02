using System.CommandLine;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.RunJit.Cleanup.Code;
using RunJit.Cli.RunJit.Fix.EmbededResources;

namespace RunJit.Cli.RunJit.Cleanup
{
    public static class AddCleanupCommandBuilderExtension
    {
        public static void AddCleanupCommandBuilder(this IServiceCollection services)
        {
            services.AddCleanupCodeCommandBuilder();

            services.AddSingletonIfNotExists<IRunJitSubCommandBuilder, CleanupCommandBuilder>();
        }
    }

    internal class CleanupCommandBuilder(IEnumerable<ICleanupSubCommandBuilder> subCommandBuilders) : IRunJitSubCommandBuilder
    {
        public Command Build()
        {
            var command = new Command("cleanup", "Ultimate code cleanup");
            subCommandBuilders.ForEach(x => command.AddCommand(x.Build()));

            return command;
        }
    }
}
