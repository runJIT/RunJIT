using System.Collections.Immutable;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Localize.Strings
{
    public static class AddLocalizeStringsParametersExtension
    {
        public static void AddLocalizeStringsParameters(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<LocalizeStringsParameters>();
        }
    }

    internal record LocalizeStringsParameters(string SolutionFile,
                                              string GitRepos,
                                              string WorkingDirectory,
                                              IImmutableList<string> Languages);
}
