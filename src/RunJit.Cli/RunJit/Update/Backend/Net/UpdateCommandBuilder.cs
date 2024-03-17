using System.CommandLine;
using System.CommandLine.Invocation;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Update.Backend.Net
{
    public static class AddDotNetCommandBuilderExtension
    {
        public static void AddDotNetCommandBuilder(this IServiceCollection services)
        {
            services.AddDotNetOptionsBuilder();
            services.AddDotNetArgumentsBuilder();
            services.AddDotNetService();

            services.AddSingletonIfNotExists<IBackendSubCommandBuilder, DotNetCommandBuilder>();
        }
    }

    internal class DotNetCommandBuilder(IDotNetService updateService,
                                        IDotNetArgumentsBuilder argumentsBuilder,
                                        IDotNetOptionsBuilder optionsBuilder) : IBackendSubCommandBuilder
    {
        public Command Build()
        {
            var command = new Command(".net", "Update the .Net framework");
            optionsBuilder.Build().ToList().ForEach(option => command.AddOption(option));
            argumentsBuilder.Build().ToList().ForEach(argument => command.AddArgument(argument));
            command.Handler = CommandHandler.Create<string, int>((solutionFile, version) => updateService.HandleAsync(new DotNetParameters(solutionFile, version)));
            return command;
        }
    }
}
