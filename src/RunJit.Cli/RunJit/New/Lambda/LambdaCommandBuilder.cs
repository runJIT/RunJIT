using System.CommandLine;
using System.CommandLine.Invocation;
using Extensions.Pack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.RunJit.Update.CodeRules;

namespace RunJit.Cli.RunJit.New.Lambda
{
    public static class AddLambdaCommandBuilderExtension
    {
        public static void AddLambdaCommandBuilder(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddLambdaOptionsBuilder();
            services.AddNewLambdaService(configuration);

            services.AddSingletonIfNotExists<INewSubCommandBuilder, LambdaCommandBuilder>();
        }
    }

    internal class LambdaCommandBuilder(IAddNewLambdaService lambdaService, ILambdaOptionsBuilder optionsBuilder) : INewSubCommandBuilder
    {
        public Command Build()
        {
            var command = new Command("lambda", "Command to create a new aws lambda");
            optionsBuilder.Build().ToList().ForEach(option => command.AddOption(option));
            command.Handler = CommandHandler.Create<FileInfo, string, string, string, string, string, string>((solution, moduleName, functionName, lambdaName, gitRepos, branch, workingDirectory) => lambdaService.HandleAsync(new LambdaParameters(solution, moduleName, functionName, lambdaName, gitRepos, branch, workingDirectory)));
            return command;
        }
    }
}
