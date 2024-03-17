using System.Collections.Immutable;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.ErrorHandling;

namespace RunJit.Cli.RunJit.Update.Backend.SwaggerTests
{
    public static class AddUpdateSwaggerTestsExtension
    {
        public static void AddUpdateSwaggerTests(this IServiceCollection services)
        {
            services.AddConsoleService();
            services.AddUpdateSwaggerTestsParameters();

            services.AddUpdateLocalSolutionFile();
            services.AddCloneReposAndUpdateAll();

            services.AddSingletonIfNotExists<IUpdateSwaggerTests, UpdateSwaggerTests>();
        }
    }

    internal interface IUpdateSwaggerTests
    {
        Task HandleAsync(UpdateSwaggerTestsParameters parameters);
    }

    internal class UpdateSwaggerTests(IEnumerable<IUpdateSwaggerTestsStrategy> updateSwaggerTestsStrategies) : IUpdateSwaggerTests
    {
        public Task HandleAsync(UpdateSwaggerTestsParameters parameters)
        {
            var updateSwaggerTestsStrategy = updateSwaggerTestsStrategies.Where(x => x.CanHandle(parameters)).ToImmutableList();
            if (updateSwaggerTestsStrategy.Count < 1)
            {
                throw new RunJitException($"Could not find a strategy a update nuget strategy for parameters: {parameters}");
            }

            if (updateSwaggerTestsStrategy.Count > 1)
            {
                throw new RunJitException($"Found more than one strategy a update nuget strategy for parameters: {parameters}");
            }

            return updateSwaggerTestsStrategy[0].HandleAsync(parameters);
        }
    }


    interface IUpdateSwaggerTestsStrategy
    {
        bool CanHandle(UpdateSwaggerTestsParameters parameters);

        Task HandleAsync(UpdateSwaggerTestsParameters parameters);
    }
}
