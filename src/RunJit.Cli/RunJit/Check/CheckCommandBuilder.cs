using System.CommandLine;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.RunJit.Check.Backend;

namespace RunJit.Cli.RunJit.Check
{
    internal static class AddCheckCommandBuilderExtension
    {
        internal static void AddCheckCommandBuilder(this IServiceCollection services)
        {
            services.AddCheckBackendCommandBuilder();

            services.AddSingletonIfNotExists<IRunJitSubCommandBuilder, CheckCommandBuilder>();
        }
    }

    internal sealed class CheckCommandBuilder(IEnumerable<ICheckSubCommandBuilder> checkSubCommandBuilders)
        : IRunJitSubCommandBuilder
    {
        public Command Build()
        {
            var checkCommand = new Command("check", "The command to check that all backends are buildable. Why we need it. Cause if new .Net updates comes out it could be new analyzer finds issues which do not before.");
            checkSubCommandBuilders.ToList().ForEach(builder => checkCommand.AddCommand(builder.Build()));

            return checkCommand;
        }
    }
}
