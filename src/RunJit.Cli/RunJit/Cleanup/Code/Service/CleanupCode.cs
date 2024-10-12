using System.Collections.Immutable;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.ErrorHandling;
using RunJit.Cli.Services;

namespace RunJit.Cli.RunJit.Cleanup.Code
{
    public static class AddCleanupCodeExtension
    {
        public static void AddCleanupCode(this IServiceCollection services)
        {
            services.AddConsoleService();
            services.AddCleanupCodeParameters();

            services.AddUpdateLocalSolutionFile();
            services.AddCloneReposAndUpdateAll();

            services.AddSingletonIfNotExists<ICleanupCode, CleanupCode>();
        }
    }

    internal interface ICleanupCode
    {
        Task HandleAsync(CleanupCodeParameters parameters);
    }

    internal class CleanupCode(IEnumerable<ICleanupCodeStrategy> fixServiceRegistrationsStrategies) : ICleanupCode
    {
        public Task HandleAsync(CleanupCodeParameters parameters)
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

    interface ICleanupCodeStrategy
    {
        bool CanHandle(CleanupCodeParameters parameters);

        Task HandleAsync(CleanupCodeParameters parameters);
    }
}
