using System.CommandLine;
using System.CommandLine.Invocation;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Update.SwaggerTests
{
    public static class AddUpdateSwaggerTestsCommandBuilderExtension
    {
        public static void AddUpdateSwaggerTestsCommandBuilder(this IServiceCollection services)
        {
            services.AddUpdateSwaggerTestsOptionsBuilder();

            // services.AddUpdateSwaggerTestsArgumentsBuilder();
            services.AddUpdateSwaggerTests();

            services.AddSingletonIfNotExists<IUpdateSubCommandBuilder, UpdateSwaggerTestsCommandBuilder>();
        }
    }

    internal class UpdateSwaggerTestsCommandBuilder(IUpdateSwaggerTests updateService,

                                                    // IUpdateSwaggerTestsArgumentsBuilder argumentsBuilder,
                                                    IUpdateSwaggerTestsOptionsBuilder optionsBuilder) : IUpdateSubCommandBuilder
    {
        public Command Build()
        {
            var command = new Command("swaggertests", "Adds or update code rules for the given solution or git repos");
            optionsBuilder.Build().ToList().ForEach(option => command.AddOption(option));

            // argumentsBuilder.Build().ToList().ForEach(argument => command.AddArgument(argument));
            command.Handler = CommandHandler.Create<string, string, string, string>((solution,
                                                                                     gitRepos,
                                                                                     workingDirectory,
                                                                                     ignorePackages) => updateService.HandleAsync(new UpdateSwaggerTestsParameters(solution ?? string.Empty, gitRepos ?? string.Empty, workingDirectory ?? string.Empty,
                                                                                                                                                                   ignorePackages ?? string.Empty)));

            return command;
        }
    }
}
