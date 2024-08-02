using System.CommandLine;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Zip
{
    public static class AddZipOptionsBuilderExtension
    {
        public static void AddZipOptionsBuilder(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<IZipOptionsBuilder, ZipOptionsBuilder>();
        }
    }

    internal interface IZipOptionsBuilder
    {
        IEnumerable<Option> Build();
    }

    internal sealed class ZipOptionsBuilder : IZipOptionsBuilder
    {
        public IEnumerable<Option> Build()
        {
            yield return BuildZipFileOption();
        }

        private Option BuildZipFileOption()
        {
            return new Option(new[] { "--zip-file", "-zf" }, "The full file path for the zip file")
                   {
                       Argument = new Argument<FileInfo>("zipFile") { Description = "The target zip file info." },
                       Required = true
                   };
        }
    }
}
