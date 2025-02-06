using System.Collections.Immutable;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.ErrorHandling;
using RunJit.Cli.Services;
using RunJit.Cli.Update.Strategies;

namespace RunJit.Cli.Update
{
    internal static class AddUpdateDotNetVersionExtension
    {
        internal static void AddUpdateDotNetVersion(this IServiceCollection services)
        {
            services.AddConsoleService();
            services.AddUpdateDotNetVersionParameters();

            services.AddCloneReposAndUpdate();
            services.AddUpdateLocalSolutionFile();

            services.AddSingletonIfNotExists<IUpdateDotNetVersion, UpdateDotNetVersion>();
        }
    }

    internal interface IUpdateDotNetVersion
    {
        Task HandleAsync(UpdateDotNetVersionParameters parameters);
    }

    internal sealed class UpdateDotNetVersion(IEnumerable<IUpdateDotNetVersionStrategy> updateDotNetVersionStrategies) : IUpdateDotNetVersion
    {
        public Task HandleAsync(UpdateDotNetVersionParameters parameters)
        {
            var updateDotNetVersionStrategy = updateDotNetVersionStrategies.Where(x => x.CanHandle(parameters)).ToImmutableList();

            if (updateDotNetVersionStrategy.Count < 1)
            {
                throw new RunJitException($"Could not find a strategy a update nuget strategy for parameters: {parameters}");
            }

            if (updateDotNetVersionStrategy.Count > 1)
            {
                throw new RunJitException($"Found more than one strategy a update nuget strategy for parameters: {parameters}");
            }

            return updateDotNetVersionStrategy[0].HandleAsync(parameters);
        }
    }

    internal interface IUpdateDotNetVersionStrategy
    {
        bool CanHandle(UpdateDotNetVersionParameters parameters);

        Task HandleAsync(UpdateDotNetVersionParameters parameters);
    }
}
