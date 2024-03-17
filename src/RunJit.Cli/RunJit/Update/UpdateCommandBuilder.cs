using System.CommandLine;
using System.CommandLine.Invocation;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.RunJit.Update.Backend;

namespace RunJit.Cli.RunJit.Update
{
    public static class AddUpdateCommandBuilderExtension
    {
        public static void AddUpdateCommandBuilder(this IServiceCollection services)
        {
            services.AddUpdateOptionsBuilder();

            services.AddBackendCommandBuilder();
            
            services.AddSingletonIfNotExists<IRunJitSubCommandBuilder, UpdateCommandBuilder>();
        }
    }

    internal class UpdateCommandBuilder(IUpdateOptionsBuilder optionsBuilder,
                                        IEnumerable<IUpdateSubCommandBuilder> subCommandBuilders)
        : IRunJitSubCommandBuilder
    {
        public Command Build()
        {
            var command = new Command("update", "Update the ultimate RunJit.Cli");
            optionsBuilder.Build().ToList().ForEach(option => command.AddOption(option));
            subCommandBuilders.ForEach(x => command.AddCommand(x.Build()));
            return command;
        }
    }
}
