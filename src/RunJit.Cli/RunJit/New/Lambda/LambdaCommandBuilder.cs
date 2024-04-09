using System.CommandLine;
using System.CommandLine.Invocation;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.New.Lambda
{
    public static class AddLambdaCommandBuilderExtension
    {
        public static void AddLambdaCommandBuilder(this IServiceCollection services)
        {
            services.AddLambdaOptionsBuilder();
            services.AddLambdaService();

            services.AddSingletonIfNotExists<INewSubCommandBuilder, LambdaCommandBuilder>();
        }
    }

    internal class LambdaCommandBuilder(ILambdaService lambdaService, ILambdaOptionsBuilder optionsBuilder) : INewSubCommandBuilder
    {
        public Command Build()
        {
            var command = new Command("lambda", "Command to create a new aws lambda");
            optionsBuilder.Build().ToList().ForEach(option => command.AddOption(option));
            command.Handler = CommandHandler.Create<FileInfo, string, string, string>((solution, moduleName, functionName, lambdaName) => lambdaService.HandleAsync(new LambdaParameters(solution, moduleName, functionName, lambdaName)));
            return command;
        }
    }
}
