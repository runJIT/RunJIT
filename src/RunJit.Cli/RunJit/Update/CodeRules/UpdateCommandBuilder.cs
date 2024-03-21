using System.CommandLine;
using System.CommandLine.Invocation;
using Extensions.Pack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.RunJit.Update.Backend;

namespace RunJit.Cli.RunJit.Update.CodeRules
{
    public static class AddUpdateCodeRulesCommandBuilderExtension
    {
        public static void AddUpdateCodeRulesCommandBuilder(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddUpdateCodeRulesOptionsBuilder();
            // services.AddUpdateCodeRulesArgumentsBuilder();
            services.AddUpdateCodeRules(configuration);

            services.AddSingletonIfNotExists<IUpdateSubCommandBuilder, UpdateCodeRulesCommandBuilder>();
        }
    }

    internal class UpdateCodeRulesCommandBuilder(IUpdateCodeRules updateService,
                                                IUpdateCodeRulesOptionsBuilder optionsBuilder) : IUpdateSubCommandBuilder
    {
        public Command Build()
        {
            var command = new Command("coderules", "Adds or update code rules for the given solution or git repos");
            optionsBuilder.Build().ToList().ForEach(option => command.AddOption(option));
            // argumentsBuilder.Build().ToList().ForEach(argument => command.AddArgument(argument));
            command.Handler = CommandHandler.Create<string, string, string, string>((solution, gitRepos, workingDirectory, ignorePackages) => updateService.HandleAsync(new UpdateCodeRulesParameters(solution ?? string.Empty, gitRepos ?? string.Empty, workingDirectory ?? string.Empty, ignorePackages ?? string.Empty)));
            return command;
        }
    }
}
