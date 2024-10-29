using System.CommandLine;
using Extensions.Pack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.RunJit.New.Lambda;

namespace RunJit.Cli.RunJit.New
{
    internal static class AddNewCommandBuilderExtension
    {
        internal static void AddNewCommandBuilder(this IServiceCollection services,
                                                IConfiguration configuration)
        {
            services.AddLambdaCommandBuilder();

            services.AddSingletonIfNotExists<IRunJitSubCommandBuilder, NewCommandBuilder>();
        }
    }

    internal sealed class NewCommandBuilder(IEnumerable<INewSubCommandBuilder> newSubCommandBuilders) : IRunJitSubCommandBuilder
    {
        public Command Build()
        {
            var newCommand = new Command("new", "The command to create a new solution, project, lambda and many more");
            newSubCommandBuilders.ToList().ForEach(builder => newCommand.AddCommand(builder.Build()));

            return newCommand;
        }
    }
}
