using System.CommandLine;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Zip
{
    internal static class AddZipArgumentsBuilderExtension
    {
        internal static void AddZipArgumentsBuilder(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<IZipArgumentsBuilder, ZipArgumentsBuilder>();
        }
    }

    internal interface IZipArgumentsBuilder
    {
        IEnumerable<System.CommandLine.Argument> Build();
    }

    internal sealed class ZipArgumentsBuilder : IZipArgumentsBuilder
    {
        public IEnumerable<System.CommandLine.Argument> Build()
        {
            yield return BuildValueArgument();
        }

        private System.CommandLine.Argument BuildValueArgument()
        {
            return new Argument<DirectoryInfo>("directory")
                   {
                       Description = "The value which should be Ziped",
                   };
        }
    }
}
