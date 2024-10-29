using System.CommandLine;
using System.CommandLine.Invocation;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Cleanup.Code
{
    internal static class AddCleanupCodeCommandBuilderExtension
    {
        internal static void AddCleanupCodeCommandBuilder(this IServiceCollection services)
        {
            services.AddCleanupCodeOptionsBuilder();

            // services.AddCleanupCodeArgumentsBuilder();
            services.AddCleanupCode();

            services.AddSingletonIfNotExists<ICleanupSubCommandBuilder, CleanupCodeCommandBuilder>();
        }
    }

    internal sealed class CleanupCodeCommandBuilder(ICleanupCode updateService,

                                             // ICleanupCodeArgumentsBuilder argumentsBuilder,
                                             ICleanupCodeOptionsBuilder optionsBuilder) : ICleanupSubCommandBuilder
    {
        public Command Build()
        {
            var command = new Command("code", "Cleanup code depdendent on editor.config and *MySolution.sln.DotSettings file");
            optionsBuilder.Build().ToList().ForEach(option => command.AddOption(option));

            // argumentsBuilder.Build().ToList().ForEach(argument => command.AddArgument(argument));
            command.Handler = CommandHandler.Create<string, string, string, string>((solution,
                                                                                     gitRepos,
                                                                                     workingDirectory,
                                                                                     ignorePackages) => updateService.HandleAsync(new CleanupCodeParameters(solution ?? string.Empty, gitRepos ?? string.Empty, workingDirectory ?? string.Empty,
                                                                                                                                                            ignorePackages ?? string.Empty)));

            return command;
        }
    }
}
