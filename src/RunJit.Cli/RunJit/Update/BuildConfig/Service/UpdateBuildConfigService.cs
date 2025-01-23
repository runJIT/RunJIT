using System.Collections.Immutable;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.ErrorHandling;
using RunJit.Cli.Services;

namespace RunJit.Cli.RunJit.Update.BuildConfig
{
    internal static class AddUUpdateBuildConfigServiceExtension
    {
        internal static void AddUpdateBuildConfigService(this IServiceCollection services)
        {
            services.AddConsoleService();
            services.AddUpdateBuildConfigParameters();

            services.AddUpdateLocalSolutionFile();
            services.AddCloneReposAndUpdateAll();

            services.AddSingletonIfNotExists<IUpdateBuildConfigService, UpdateBuildConfigServiceService>();
        }
    }

    internal interface IUpdateBuildConfigService
    {
        Task HandleAsync(UpdateBuildConfigParameters parameters);
    }

    internal sealed class UpdateBuildConfigServiceService(IEnumerable<IUpdateBuildConfigStrategy> updateBuildConfigStrategies) : IUpdateBuildConfigService
    {
        public Task HandleAsync(UpdateBuildConfigParameters parameters)
        {
            var updateBuildConfigStrategy = updateBuildConfigStrategies.Where(x => x.CanHandle(parameters)).ToImmutableList();

            if (updateBuildConfigStrategy.Count < 1)
            {
                throw new RunJitException($"Could not find a strategy a update nuget strategy for parameters: {parameters}");
            }

            if (updateBuildConfigStrategy.Count > 1)
            {
                throw new RunJitException($"Found more than one strategy a update nuget strategy for parameters: {parameters}");
            }

            return updateBuildConfigStrategy[0].HandleAsync(parameters);
        }
    }

    internal interface IUpdateBuildConfigStrategy
    {
        bool CanHandle(UpdateBuildConfigParameters parameters);

        Task HandleAsync(UpdateBuildConfigParameters parameters);
    }
}
