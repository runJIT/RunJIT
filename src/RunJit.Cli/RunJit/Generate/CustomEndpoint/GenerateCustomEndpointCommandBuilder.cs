using System.CommandLine;
using System.CommandLine.Invocation;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Generate.CustomEndpoint
{
    public static class AddGenerateCustomEndpointCommandBuilderExtension
    {
        public static void AddGenerateCustomEndpointCommandBuilder(this IServiceCollection services)
        {
            services.AddGenerateCustomEndpointOptionsBuilder();
            services.AddGenerateCustomEndointService();

            services.AddSingletonIfNotExists<IGenerateSubCommandBuilder, GenerateCustomEndpointCommandBuilder>();
        }
    }

    internal sealed class GenerateCustomEndpointCommandBuilder(GenerateCustomEndpointService generateEndpointService,
                                                               IGenerateEndpointOptionsBuilder optionsBuilder)
        : IGenerateSubCommandBuilder
    {
        public Command Build()
        {
            var command = new Command("custom-endpoint", "The command to create a new pulse web api solution");
            optionsBuilder.Build().ToList().ForEach(option => command.AddOption(option));

            command.Handler = CommandHandler.Create<DirectoryInfo, string, bool>((targetFolder,
                                                                                  endpointData,
                                                                                  overwriteCode) =>
                                                                                 {
                                                                                     var parameters = new GenerateCustomEndpointParameters(targetFolder, endpointData, overwriteCode);

                                                                                     return generateEndpointService.GenerateAsync(parameters);
                                                                                 });

            return command;
        }
    }
}
