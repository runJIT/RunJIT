using System.CommandLine;
using System.CommandLine.Invocation;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.RunJit.Check.Backend;

namespace RunJit.Cli.RunJit.Localize.Strings
{
    public static class AddLocalizeStringsCommandBuilderExtension
    {
        public static void AddLocalizeStringsCommandBuilder(this IServiceCollection services)
        {
            services.AddLocalizeStringsArgumentsBuilder();
            services.AddLocalizeStringsOptionsBuilder();
            services.AddLocalizeStrings();

            services.AddSingletonIfNotExists<ILocalizeSubCommandBuilder, LocalizeStringsCommandBuilder>();
        }
    }

    internal sealed class LocalizeStringsCommandBuilder(ILocalizeStringsOptionsBuilder LocalizeStringsOptionsBuilder,
                                                        ILocalizeStrings LocalizeStrings) : ILocalizeSubCommandBuilder
    {
        public Command Build()
        {
            var checkCommand = new Command("strings", "The command to localize all strings in the passed solution");

            //LocalizeStringsArgumentsBuilder.Build().ForEach(arg => checkCommand.AddArgument(arg));
            LocalizeStringsOptionsBuilder.Build().ForEach(opt => checkCommand.AddOption(opt));

            checkCommand.Handler = CommandHandler.Create<string, string, string, string>((solution,
                                                                                  gitRepos,
                                                                                  workingDirectory,
                                                                                  languages) => LocalizeStrings.HandleAsync(new LocalizeStringsParameters(solution ?? string.Empty, gitRepos ?? string.Empty, workingDirectory ?? string.Empty, languages?.Split(";") ?? new[] {"de", "en"})));

            return checkCommand;
        }
    }
}
