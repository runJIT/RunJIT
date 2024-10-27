using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Localize.Strings
{
    public static class AddLocalizeStringsArgumentsBuilderExtension
    {
        public static void AddLocalizeStringsArgumentsBuilder(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<ILocalizeStringsArgumentsBuilder, LocalizeStringsArgumentsBuilder>();
        }
    }

    internal interface ILocalizeStringsArgumentsBuilder
    {
        IEnumerable<System.CommandLine.Argument> Build();
    }

    internal sealed class LocalizeStringsArgumentsBuilder : ILocalizeStringsArgumentsBuilder
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
