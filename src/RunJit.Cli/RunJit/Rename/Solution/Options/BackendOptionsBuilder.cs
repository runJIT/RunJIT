using System.CommandLine;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Rename.Solution
{
    public static class AddBackendOptionsBuilderExtension
    {
        public static void AddBackendOptionsBuilder(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<IBackendOptionsBuilder, BackendOptionsBuilder>();
        }
    }

    internal interface IBackendOptionsBuilder
    {
        IEnumerable<Option> Build();
    }

    internal class BackendOptionsBuilder : IBackendOptionsBuilder
    {
        public IEnumerable<Option> Build()
        {
            yield return BuildOldNameOption();
            yield return BuildNewNameOption();
        }

        public Option BuildOldNameOption()
        {
            return new Option(new[] { "--old-name", "-on" }, "The old or current name of your backend. Sample RunJit.Cli or Simple.WebApi")
                   {
                       Required = true,
                       Argument = new Argument<string>("oldName") { Description = "The old or current name of your backend. Sample RunJit.Cli or Simple.WebApi" }
                   };
        }

        public Option BuildNewNameOption()
        {
            return new Option(new[] { "--new-name", "-nn" }, "The new name of your backend. Sample RunJit.Cli or Simple.WebApi")
                   {
                       Required = true,
                       Argument = new Argument<string>("newName") { Description = "The new name of your backend. Sample RunJit.Cli or Simple.WebApi" }
                   };
        }
    }
}
