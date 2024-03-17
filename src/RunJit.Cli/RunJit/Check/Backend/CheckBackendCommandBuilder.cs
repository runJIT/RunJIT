using System.CommandLine;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.RunJit.Check.Backend.Builds;

namespace RunJit.Cli.RunJit.Check.Backend
{
    public static class AddCheckBackendCommandBuilderExtension
    {
        public static void AddCheckBackendCommandBuilder(this IServiceCollection services)
        {
            services.AddCheckBackendBuildsCommandBuilder();

            services.AddSingletonIfNotExists<ICheckSubCommandBuilder, CheckBackendCommandBuilder>();
        }
    }

    internal sealed class CheckBackendCommandBuilder(IEnumerable<ICheckBackendSubCommandBuilder> checkBackendSubCommandBuilders) : ICheckSubCommandBuilder
    {
        public Command Build()
        {
            var checkBackendCommand = new Command("backend", "The command to check or proof some specific conditions on backends.");
            checkBackendSubCommandBuilders.ForEach(builder => checkBackendCommand.AddCommand(builder.Build()));
            return checkBackendCommand;
        }
    }
}
