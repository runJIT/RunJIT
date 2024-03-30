using System.CommandLine;
using System.CommandLine.Invocation;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Update.Backend.CodeRules
{
    public static class AddUpdateCodeRulesCommandBuilderExtension
    {
        public static void AddUpdateCodeRulesCommandBuilder(this IServiceCollection services)
        {
            services.AddUpdateCodeRulesOptionsBuilder();
            // services.AddUpdateCodeRulesArgumentsBuilder();
            services.AddUpdateCodeRules();

            services.AddSingletonIfNotExists<IBackendSubCommandBuilder, UpdateCodeRulesCommandBuilder>();
        }
    }

    internal class UpdateCodeRulesCommandBuilder(IUpdateCodeRules updateService,
                                        // IUpdateCodeRulesArgumentsBuilder argumentsBuilder,
                                        IUpdateCodeRulesOptionsBuilder optionsBuilder) : IBackendSubCommandBuilder
    {
        public Command Build()
        {
            var command = new Command("coderules", "Adds or update code rules for the given solution or git repos");
            optionsBuilder.Build().ToList().ForEach(option => command.AddOption(option));
            // argumentsBuilder.Build().ToList().ForEach(argument => command.AddArgument(argument));
            command.Handler = CommandHandler.Create<string, string, string, string, string>((solution, gitRepos, workingDirectory, ignorePackages, branch) => updateService.HandleAsync(new UpdateCodeRulesParameters(solution ?? string.Empty, gitRepos ?? string.Empty, workingDirectory ?? string.Empty, ignorePackages ?? string.Empty, branch ?? string.Empty)));
            return command;
        }
    }
}
