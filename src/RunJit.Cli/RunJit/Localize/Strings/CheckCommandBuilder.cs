using System.Collections.Immutable;
using System.CommandLine;
using System.CommandLine.Invocation;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Localize.Strings
{
    internal static class AddLocalizeStringsCommandBuilderExtension
    {
        internal static void AddLocalizeStringsCommandBuilder(this IServiceCollection services)
        {
            services.AddLocalizeStringsArgumentsBuilder();
            services.AddLocalizeStringsOptionsBuilder();
            services.AddLocalizeStrings();

            services.AddSingletonIfNotExists<ILocalizeSubCommandBuilder, LocalizeStringsCommandBuilder>();
        }
    }

    internal sealed class LocalizeStringsCommandBuilder(ILocalizeStringsOptionsBuilder localizeStringsOptionsBuilder,
                                                        ILocalizeStrings localizeStrings) : ILocalizeSubCommandBuilder
    {
        public Command Build()
        {
            var checkCommand = new Command("strings", "The command to localize all strings in the passed solution");

            //LocalizeStringsArgumentsBuilder.Build().ForEach(arg => checkCommand.AddArgument(arg));
            localizeStringsOptionsBuilder.Build().ForEach(opt => checkCommand.AddOption(opt));

            checkCommand.Handler = CommandHandler.Create<string, string, string, string>((solution,
                                                                                          gitRepos,
                                                                                          workingDirectory,
                                                                                          languages) => localizeStrings.HandleAsync(new LocalizeStringsParameters(solution ?? string.Empty, gitRepos ?? string.Empty, workingDirectory ?? string.Empty,
                                                                                                                                                                  languages?.Split(";").ToImmutableList() ?? ImmutableList.Create<string>("de", "en"))));

            return checkCommand;
        }
    }
}
