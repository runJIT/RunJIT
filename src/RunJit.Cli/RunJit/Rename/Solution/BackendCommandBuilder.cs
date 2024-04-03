using System.CommandLine;
using System.CommandLine.Invocation;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Rename.Solution
{
    public static class AddBackendCommandBuilderExtension
    {
        public static void AddBackendCommandBuilder(this IServiceCollection services)
        {
            services.AddBackendOptionsBuilder();
            services.AddBackendArgumentsBuilder();
            services.AddBackendService();
            
            services.AddSingletonIfNotExists<IRenameSubCommandBuilder, BackendCommandBuilder>();
        }
    }

    internal class BackendCommandBuilder(IBackendOptionsBuilder backendOptionsBuilder,
                                         IBackendArgumentsBuilder backendArgumentsBuilder,
                                         IBackendService backendService) : IRenameSubCommandBuilder
    {
        public Command Build()
        {
            var command = new Command("solution", "Commands to do some actions on backend");
            backendOptionsBuilder.Build().ForEach(x => command.AddOption(x));
            backendArgumentsBuilder.Build().ForEach(x => command.AddArgument(x));
            command.Handler = CommandHandler.Create<string, string, string>((fileOrFolder, oldName, newName) => backendService.HandleAsync(new BackendParameters(fileOrFolder, oldName, newName)));
            return command;
        }
    }
}
