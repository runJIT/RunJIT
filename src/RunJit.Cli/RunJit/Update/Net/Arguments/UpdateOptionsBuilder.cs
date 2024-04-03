using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Update.Net
{
    public static class AddDotNetArgumentsBuilderExtension
    {
        public static void AddDotNetArgumentsBuilder(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<IDotNetArgumentsBuilder, DotNetArgumentsBuilder>();
        }
    }

    internal interface IDotNetArgumentsBuilder
    {
        IEnumerable<System.CommandLine.Argument> Build();
    }

    internal class DotNetArgumentsBuilder : IDotNetArgumentsBuilder
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
