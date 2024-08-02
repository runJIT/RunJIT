using System.CommandLine;
using System.CommandLine.Invocation;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Update.ResharperSettings
{
    public static class AddUpdateResharperSettingsCommandBuilderExtension
    {
        public static void AddUpdateResharperSettingsCommandBuilder(this IServiceCollection services)
        {
            services.AddUpdateResharperSettingsOptionsBuilder();

            // services.AddUpdateResharperSettingsArgumentsBuilder();
            services.AddUpdateResharperSettings();

            services.AddSingletonIfNotExists<IUpdateSubCommandBuilder, UpdateResharperSettingsCommandBuilder>();
        }
    }

    internal class UpdateResharperSettingsCommandBuilder(IUpdateResharperSettings updateService,

                                                         // IUpdateResharperSettingsArgumentsBuilder argumentsBuilder,
                                                         IUpdateResharperSettingsOptionsBuilder optionsBuilder) : IUpdateSubCommandBuilder
    {
        public Command Build()
        {
            var command = new Command("resharpersettings", "Adds or update code rules for the given solution or git repos");
            optionsBuilder.Build().ToList().ForEach(option => command.AddOption(option));

            // argumentsBuilder.Build().ToList().ForEach(argument => command.AddArgument(argument));
            command.Handler = CommandHandler.Create<string, string, string, string>((solution,
                                                                                     gitRepos,
                                                                                     workingDirectory,
                                                                                     ignorePackages) => updateService.HandleAsync(new UpdateResharperSettingsParameters(solution ?? string.Empty, gitRepos ?? string.Empty, workingDirectory ?? string.Empty,
                                                                                                                                                                        ignorePackages ?? string.Empty)));

            return command;
        }
    }
}
