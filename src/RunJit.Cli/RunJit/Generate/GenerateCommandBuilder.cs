using System.CommandLine;
using Extensions.Pack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Generate.DotNetTool;
using RunJit.Cli.RunJit.Generate.Client;
using RunJit.Cli.RunJit.Generate.CustomEndpoint;

namespace RunJit.Cli.RunJit.Generate
{
    internal static class AddGenerateCommandBuilderExtension
    {
        internal static void AddGenerateCommandBuilder(this IServiceCollection services,
                                                       IConfiguration configuration)
        {
            services.AddClientCommandBuilder(configuration);
            services.AddGenerateCustomEndpointCommandBuilder();
            services.AddDotNetToolCommandBuilder();

            services.AddSingletonIfNotExists<IRunJitSubCommandBuilder, GenerateCommandBuilder>();
        }
    }

    internal sealed class GenerateCommandBuilder(IEnumerable<IGenerateSubCommandBuilder> generateSubCommandBuilders) : IRunJitSubCommandBuilder
    {
        public Command Build()
        {
            var generateCommand = new Command("generate", "The command to generate something like a client, tests or any other cool things");
            generateSubCommandBuilders.ToList().ForEach(builder => generateCommand.AddCommand(builder.Build()));

            return generateCommand;
        }
    }
}
