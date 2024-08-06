using System.Collections.Immutable;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.ErrorHandling;

namespace RunJit.Cli.RunJit.Update.BuildConfig
{
    public static class AddUpdateBuildConfigExtension
    {
        public static void AddUpdateBuildConfig(this IServiceCollection services)
        {
            services.AddConsoleService();
            services.AddUpdateBuildConfigParameters();

            services.AddUpdateLocalSolutionFile();
            services.AddCloneReposAndUpdateAll();

            services.AddSingletonIfNotExists<IUpdateBuildConfig, UpdateBuildConfig>();
        }
    }

    internal interface IUpdateBuildConfig
    {
        Task HandleAsync(UpdateBuildConfigParameters parameters);
    }

    internal class UpdateBuildConfig(IEnumerable<IUpdateBuildConfigStrategy> UpdateBuildConfigStrategies) : IUpdateBuildConfig
    {
        public Task HandleAsync(UpdateBuildConfigParameters parameters)
        {
            var UpdateBuildConfigStrategy = UpdateBuildConfigStrategies.Where(x => x.CanHandle(parameters)).ToImmutableList();

            if (UpdateBuildConfigStrategy.Count < 1)
            {
                throw new RunJitException($"Could not find a strategy a update nuget strategy for parameters: {parameters}");
            }

            if (UpdateBuildConfigStrategy.Count > 1)
            {
                throw new RunJitException($"Found more than one strategy a update nuget strategy for parameters: {parameters}");
            }

            return UpdateBuildConfigStrategy[0].HandleAsync(parameters);
        }
    }

    interface IUpdateBuildConfigStrategy
    {
        bool CanHandle(UpdateBuildConfigParameters parameters);

        Task HandleAsync(UpdateBuildConfigParameters parameters);
    }
}
