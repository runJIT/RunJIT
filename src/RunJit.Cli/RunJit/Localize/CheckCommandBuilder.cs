using System.CommandLine;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.RunJit.Localize.Strings;

namespace RunJit.Cli.RunJit.Localize
{
    public static class AddLocalizeCommandBuilderExtension
    {
        public static void AddLocalizeCommandBuilder(this IServiceCollection services)
        {
            services.AddLocalizeStringsCommandBuilder();

            services.AddSingletonIfNotExists<IRunJitSubCommandBuilder, LocalizeCommandBuilder>();
        }
    }

    internal sealed class LocalizeCommandBuilder(IEnumerable<ILocalizeSubCommandBuilder> localizeSubCommandBuilders)
        : IRunJitSubCommandBuilder
    {
        public Command Build()
        {
            var localizeCommand = new Command("localize", "The command to localize an application, strings or any other expected source");
            localizeSubCommandBuilders.ToList().ForEach(builder => localizeCommand.AddCommand(builder.Build()));

            return localizeCommand;
        }
    }
}
