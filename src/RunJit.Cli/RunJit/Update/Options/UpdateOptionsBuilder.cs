using System.CommandLine;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Update
{
    public static class AddUpdateOptionsBuilderExtension
    {
        public static void AddUpdateOptionsBuilder(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<IUpdateOptionsBuilder, UpdateOptionsBuilder>();
        }
    }

    internal interface IUpdateOptionsBuilder
    {
        IEnumerable<Option> Build();
    }

    internal class UpdateOptionsBuilder : IUpdateOptionsBuilder
    {
        public IEnumerable<Option> Build()
        {
            yield return BuildSourceOption();
        }

        public Option BuildSourceOption()
        {
            return new Option(new[] { "--version", "-v" }, "The nuget version which should be updated to")
            {
                Required = false,
                Argument = new Argument<string>("version")
                {
                    Description = "The nuget version which should be updated to"
                }
            };
        }
    }
}
