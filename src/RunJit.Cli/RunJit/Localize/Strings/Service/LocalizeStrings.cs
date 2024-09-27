using System.Collections.Immutable;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.ErrorHandling;

namespace RunJit.Cli.RunJit.Localize.Strings
{
    public static class AddLocalizeStringsExtension
    {
        public static void AddLocalizeStrings(this IServiceCollection services)
        {
            services.AddConsoleService();
            services.AddLocalizeStringsParameters();

            // services.AddUpdateLocalSolutionFile();
            services.AddCloneReposAndUpdateAll();

            services.AddSingletonIfNotExists<ILocalizeStrings, LocalizeStrings>();
        }
    }

    internal interface ILocalizeStrings
    {
        Task HandleAsync(LocalizeStringsParameters parameters);
    }

    internal class LocalizeStrings(IEnumerable<ILocalizeStringsStrategy> fixServiceRegistrationsStrategies) : ILocalizeStrings
    {
        public Task HandleAsync(LocalizeStringsParameters parameters)
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

    interface ILocalizeStringsStrategy
    {
        bool CanHandle(LocalizeStringsParameters parameters);

        Task HandleAsync(LocalizeStringsParameters parameters);
    }
}
