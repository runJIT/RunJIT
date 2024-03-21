using System.CommandLine;
using System.CommandLine.Invocation;
using Extensions.Pack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.RunJit.Update.Backend;
using RunJit.Cli.RunJit.Update.CodeRules;

namespace RunJit.Cli.RunJit.Update
{
    public static class AddUpdateCommandBuilderExtension
    {
        public static void AddUpdateCommandBuilder(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddUpdateOptionsBuilder();
            services.AddBackendCommandBuilder();
            services.AddUpdateCodeRulesCommandBuilder(configuration);
            
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
