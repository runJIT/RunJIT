using System.IO.Compression;
using Argument.Check;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal static class AddTemplateExtractorExtension
    {
        internal static void AddTemplateExtractor(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<ITemplateExtractor, TemplateExtractor>();
        }
    }

    internal interface ITemplateExtractor
    {
        void ExtractTo(DirectoryInfo directoryInfo, DotNetToolParameters clientGenParameters);
    }

    internal class TemplateExtractor : ITemplateExtractor
    {
        public void ExtractTo(DirectoryInfo directoryInfo, DotNetToolParameters clientGenParameters)
        {
            var assembly = GetType().Assembly;
            var resourceNames = assembly.GetManifestResourceNames();

            var template = "client.project.template";

            var templateResourceName = resourceNames.SingleOrDefault(resource => resource.Contains(template));

            Throw.IfNull(templateResourceName);

            using var templateStream = assembly.GetManifestResourceStream(templateResourceName);
            Throw.IfNull(templateStream);

            using var zipArchive = new ZipArchive(templateStream);
            zipArchive.ExtractToDirectory(directoryInfo.FullName, true);
        }
    }
}
