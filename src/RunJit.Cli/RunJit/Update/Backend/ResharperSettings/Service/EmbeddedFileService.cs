using System.Xml.Linq;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using Solution.Parser.Project;

namespace RunJit.Cli.RunJit.Update.Backend.ResharperSettings
{
    public static class AddEmbeddedFileServiceExtension
    {
        public static void AddEmbeddedFileService(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<EmbeddedFileService>();
        }
    }

    internal class EmbeddedFileService
    {
        internal void EmbedFile(ProjectFile projectFile, string namesapce, string file)
        {
            var withoutProject = namesapce.Replace($"{projectFile.ProjectFileInfo.Value.NameWithoutExtension()}.", string.Empty);
            var replaceDots = withoutProject.Replace(".", @"\");
            var pathToEmbedFile = $@"{replaceDots}\{file}";

            var xdocument = XDocument.Load(projectFile.ProjectFileInfo.Value.FullName);

            var elements = xdocument.ElementsBy("EmbeddedResource");
            if (elements.IsEmpty())
            {
                var itemGroup = new XElement("ItemGroup");
                var embeddedResource = new XElement("EmbeddedResource");
                embeddedResource.Add(new XAttribute("Include", pathToEmbedFile));
                itemGroup.Add(embeddedResource);
                xdocument.Root!.Add(itemGroup);
            }
            else
            {
                var parent = elements.Last().Parent!;
                var embeddedResource = new XElement("EmbeddedResource");
                embeddedResource.Add(new XAttribute("Include", pathToEmbedFile));
                parent.Add(embeddedResource);
            }

            xdocument.Save(projectFile.ProjectFileInfo.Value.FullName);
        }
    }
}
