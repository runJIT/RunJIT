using System.CommandLine;
using System.CommandLine.Invocation;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Fix.EmbededResources
{
    internal static class AddFixEmbeddedResourcesCommandBuilderExtension
    {
        internal static void AddFixEmbeddedResourcesCommandBuilder(this IServiceCollection services)
        {
            services.AddFixEmbeddedResourcesOptionsBuilder();

            // services.AddFixEmbeddedResourcesArgumentsBuilder();
            services.AddFixEmbeddedResources();

            services.AddSingletonIfNotExists<IFixSubCommandBuilder, FixEmbeddedResourcesCommandBuilder>();
        }
    }

    internal sealed class FixEmbeddedResourcesCommandBuilder(IFixEmbeddedResources updateService,

                                                             // IFixEmbeddedResourcesArgumentsBuilder argumentsBuilder,
                                                             IFixEmbeddedResourcesOptionsBuilder optionsBuilder) : IFixSubCommandBuilder
    {
        public Command Build()
        {
            var command = new Command("embeddedresource", "Detects all service and options usage and fix their registrations");
            optionsBuilder.Build().ToList().ForEach(option => command.AddOption(option));

            // argumentsBuilder.Build().ToList().ForEach(argument => command.AddArgument(argument));
            command.Handler = CommandHandler.Create<string, string, string, string>((solution,
                                                                                     gitRepos,
                                                                                     workingDirectory,
                                                                                     ignorePackages) => updateService.HandleAsync(new FixEmbeddedResourcesParameters(solution ?? string.Empty, gitRepos ?? string.Empty, workingDirectory ?? string.Empty,
                                                                                                                                                                     ignorePackages ?? string.Empty)));

            return command;
        }
    }
}
