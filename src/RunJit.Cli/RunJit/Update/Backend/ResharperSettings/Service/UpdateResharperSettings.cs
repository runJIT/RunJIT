using System.Collections.Immutable;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.ErrorHandling;

namespace RunJit.Cli.RunJit.Update.Backend.ResharperSettings
{
    public static class AddUpdateResharperSettingsExtension
    {
        public static void AddUpdateResharperSettings(this IServiceCollection services)
        {
            services.AddConsoleService();
            services.AddUpdateResharperSettingsParameters();

            services.AddCloneReposAndUpdateAll();

            services.AddSingletonIfNotExists<IUpdateResharperSettings, UpdateResharperSettings>();
        }
    }

    internal interface IUpdateResharperSettings
    {
        Task HandleAsync(UpdateResharperSettingsParameters parameters);
    }

    internal class UpdateResharperSettings(IEnumerable<IUpdateResharperSettingsStrategy> updateSwaggerTestsStrategies) : IUpdateResharperSettings
    {
        public Task HandleAsync(UpdateResharperSettingsParameters parameters)
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


    interface IUpdateResharperSettingsStrategy
    {
        bool CanHandle(UpdateResharperSettingsParameters parameters);

        Task HandleAsync(UpdateResharperSettingsParameters parameters);
    }
}
