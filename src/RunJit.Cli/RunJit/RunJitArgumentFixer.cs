using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit
{
    internal static class AddDotNetCliArgumentFixerExtension
    {
        internal static void AddDotNetCliArgumentFixer(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<RunJitArgumentFixer>();
        }
    }

    internal sealed class RunJitArgumentFixer
    {
        public string[] Fix(string[] args)
        {
            var defaultArgs = new[] { "runjit" };
            var newArgs = defaultArgs.Concat(args).Distinct().ToList();

            return newArgs.ToArray();
        }
    }
}
