using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Cleanup.Code
{
    internal static class AddCleanupCodeArgumentsBuilderExtension
    {
        internal static void AddCleanupCodeArgumentsBuilder(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<ICleanupCodeArgumentsBuilder, CleanupCodeArgumentsBuilder>();
        }
    }

    internal interface ICleanupCodeArgumentsBuilder
    {
        IEnumerable<System.CommandLine.Argument> Build();
    }

    internal sealed class CleanupCodeArgumentsBuilder : ICleanupCodeArgumentsBuilder
    {
        public IEnumerable<System.CommandLine.Argument> Build()
        {
            yield return BuildSourceOption();
        }

        public System.CommandLine.Argument BuildSourceOption()
        {
            return new System.CommandLine.Argument<string>()
            {
                Name = "solutionFile",
            };
        }
    }
}
