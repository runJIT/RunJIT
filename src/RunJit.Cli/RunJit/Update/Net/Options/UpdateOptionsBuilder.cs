using System.CommandLine;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Update.Net
{
    public static class AddDotNetOptionsBuilderExtension
    {
        public static void AddDotNetOptionsBuilder(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<IDotNetOptionsBuilder, DotNetOptionsBuilder>();
        }
    }

    internal interface IDotNetOptionsBuilder
    {
        IEnumerable<Option> Build();
    }

    internal class DotNetOptionsBuilder : IDotNetOptionsBuilder
    {
        public IEnumerable<Option> Build()
        {
            yield return BuildSourceOption();
        }

        public Option BuildSourceOption()
        {
            return new Option(new[] { "--version", "-v" }, "The .Net version which should be the target one. Sample 6,7,8")
                   {
                       Required = true,
                       Argument = new Argument<int>("version") { Description = "The .Net version which should be the target one. Sample 6,7,8" }
                   };
        }
    }
}
