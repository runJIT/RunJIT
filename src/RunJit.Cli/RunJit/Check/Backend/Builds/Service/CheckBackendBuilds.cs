using System.Collections.Immutable;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.ErrorHandling;
using RunJit.Cli.Services;

namespace RunJit.Cli.RunJit.Check.Backend.Builds
{
    public static class AddCheckBackendBuildsExtension
    {
        public static void AddCheckBackendBuilds(this IServiceCollection services)
        {
            services.AddConsoleService();
            services.AddCheckBackendBuildsParameters();

            // services.AddUpdateLocalSolutionFile();
            services.AddCloneReposAndUpdateAll();

            services.AddSingletonIfNotExists<ICheckBackendBuilds, CheckBackendBuilds>();
        }
    }

    internal interface ICheckBackendBuilds
    {
        Task HandleAsync(CheckBackendBuildsParameters parameters);
    }

    internal sealed class CheckBackendBuilds(IEnumerable<ICheckBackendBuildsStrategy> fixServiceRegistrationsStrategies) : ICheckBackendBuilds
    {
        public Task HandleAsync(CheckBackendBuildsParameters parameters)
        {
            var fixServiceRegistrationsStrategy = fixServiceRegistrationsStrategies.Where(x => x.CanHandle(parameters)).ToImmutableList();

            if (fixServiceRegistrationsStrategy.Count < 1)
            {
                throw new RunJitException($"Could not find a strategy a update nuget strategy for parameters: {parameters}");
            }

            if (fixServiceRegistrationsStrategy.Count > 1)
            {
                throw new RunJitException($"Found more than one strategy a update nuget strategy for parameters: {parameters}");
            }

            return fixServiceRegistrationsStrategy[0].HandleAsync(parameters);
        }
    }

    interface ICheckBackendBuildsStrategy
    {
        bool CanHandle(CheckBackendBuildsParameters parameters);

        Task HandleAsync(CheckBackendBuildsParameters parameters);
    }
}
