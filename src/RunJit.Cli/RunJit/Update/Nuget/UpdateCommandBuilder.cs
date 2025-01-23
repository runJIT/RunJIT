using System.CommandLine;
using System.CommandLine.Invocation;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Update.Nuget
{
    internal static class AddUpdateNugetCommandBuilderExtension
    {
        internal static void AddUpdateNugetCommandBuilder(this IServiceCollection services)
        {
            services.AddUpdateNugetOptionsBuilder();

            // services.AddUpdateNugetArgumentsBuilder();
            services.AddUpdateNuget();

            services.AddSingletonIfNotExists<IUpdateSubCommandBuilder, UpdateNugetCommandBuilder>();
        }
    }

    internal sealed class UpdateNugetCommandBuilder(IUpdateNuget updateService,

                                                    // IUpdateNugetArgumentsBuilder argumentsBuilder,
                                                    IUpdateNugetOptionsBuilder optionsBuilder) : IUpdateSubCommandBuilder
    {
        public Command Build()
        {
            var command = new Command("nuget", "Update nuget packages for .net backend(s)");
            optionsBuilder.Build().ToList().ForEach(option => command.AddOption(option));

            // argumentsBuilder.Build().ToList().ForEach(argument => command.AddArgument(argument));
            command.Handler = CommandHandler.Create<string, string, string, string>((solution,
                                                                                     gitRepos,
                                                                                     workingDirectory,
                                                                                     ignorePackages) => updateService.HandleAsync(new UpdateNugetParameters(solution ?? string.Empty, gitRepos ?? string.Empty, workingDirectory ?? string.Empty,
                                                                                                                                                            ignorePackages ?? string.Empty)));

            return command;
        }
    }
}
