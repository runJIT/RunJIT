using System.Collections.Immutable;
using AspNetCore.Simple.Sdk.Authentication.Auth0;
using Extensions.Pack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.ErrorHandling;
using RunJit.Cli.RunJit.New.Lambda;

namespace RunJit.Cli.RunJit.Update.CodeRules
{
    public static class AddNewLambdaServiceExtension
    {
        public static void AddNewLambdaService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddNewLambdaIntoLocalSolution(configuration);
            services.AddCloneRepoAndAddLambda();

            services.AddUpdateLocalSolutionFile(configuration);
            services.AddCloneReposAndUpdateAll();
            
            services.AddAuth0Settings(configuration);

            services.AddSingletonIfNotExists<IAddNewLambdaService, AddNewLambdaService>();
        }
    }

    internal interface IAddNewLambdaService
    {
        Task HandleAsync(LambdaParameters parameters);
    }

    internal class AddNewLambdaService(IEnumerable<IAddNewLambdaServiceStrategy> updateCodeRulesStrategies) : IAddNewLambdaService
    {
        public Task HandleAsync(LambdaParameters parameters)
        {
            var updateCodeRulesStrategy = updateCodeRulesStrategies.Where(x => x.CanHandle(parameters)).ToImmutableList();
            if (updateCodeRulesStrategy.Count < 1)
            {
                throw new RunJitException($"Could not find a strategy a update nuget strategy for parameters: {parameters}");
            }

            if (updateCodeRulesStrategy.Count > 1)
            {
                throw new RunJitException($"Found more than one strategy a update nuget strategy for parameters: {parameters}");
            }

            return updateCodeRulesStrategy[0].HandleAsync(parameters);
        }
    }


    interface IAddNewLambdaServiceStrategy
    {
        bool CanHandle(LambdaParameters parameters);

        Task HandleAsync(LambdaParameters parameters);
    }
}
