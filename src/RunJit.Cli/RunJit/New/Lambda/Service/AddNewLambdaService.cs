﻿using System.Collections.Immutable;
using AspNetCore.Simple.Sdk.Authentication.Auth0;
using Extensions.Pack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.ErrorHandling;
using RunJit.Cli.RunJit.Update.CodeRules;

namespace RunJit.Cli.RunJit.New.Lambda
{
    internal static class AddNewLambdaServiceExtension
    {
        internal static void AddNewLambdaService(this IServiceCollection services,
                                                 IConfiguration configuration)
        {
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

    internal sealed class AddNewLambdaService(IEnumerable<IAddNewLambdaServiceStrategy> updateCodeRulesStrategies) : IAddNewLambdaService
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

    internal interface IAddNewLambdaServiceStrategy
    {
        bool CanHandle(LambdaParameters parameters);

        Task HandleAsync(LambdaParameters parameters);
    }
}
