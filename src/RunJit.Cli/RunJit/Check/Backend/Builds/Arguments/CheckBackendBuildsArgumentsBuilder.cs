using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Check.Backend.Builds
{
    public static class AddCheckBackendBuildsArgumentsBuilderExtension
    {
        public static void AddCheckBackendBuildsArgumentsBuilder(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<ICheckBackendBuildsArgumentsBuilder, CheckBackendBuildsArgumentsBuilder>();
        }
    }

    internal interface ICheckBackendBuildsArgumentsBuilder
    {
        IEnumerable<System.CommandLine.Argument> Build();
    }

    internal sealed class CheckBackendBuildsArgumentsBuilder : ICheckBackendBuildsArgumentsBuilder
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
