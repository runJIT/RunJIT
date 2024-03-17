using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit
{
    internal static class AddDotNetCliArgumentFixerExtension
    {
        internal static void AddDotNetCliArgumentFixer(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<IDotNetCliArgumentFixer, RunJitArgumentFixer>();
        }
    }
    internal interface IDotNetCliArgumentFixer
    {
        string[] Fix(string[] args);
    }

    internal class RunJitArgumentFixer : IDotNetCliArgumentFixer
    {
        public string[] Fix(string[] args)
        {
            var defaultArgs = new[] { "runjit" };
            var newArgs = defaultArgs.Concat(args).Distinct().ToList();
            return newArgs.ToArray();
        }
    }
}
