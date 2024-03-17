using System.CommandLine;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.RunJit.Rename.Backend;

namespace RunJit.Cli.RunJit.Rename
{
    public static class AddRenameCommandBuilderExtension
    {
        public static void AddRenameCommandBuilder(this IServiceCollection services)
        {
            services.AddBackendCommandBuilder();
            
            services.AddSingletonIfNotExists<IRunJitSubCommandBuilder, RenameCommandBuilder>();
        }
    }

    internal class RenameCommandBuilder(IEnumerable<IRenameSubCommandBuilder> subCommandBuilders) : IRunJitSubCommandBuilder
    {
        public Command Build()
        {
            var command = new Command("rename", "Command to rename a specific context");
            subCommandBuilders.ForEach(x => command.AddCommand(x.Build()));
            return command;
        }
    }
}
