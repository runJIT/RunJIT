using System.Xml.Linq;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using Solution.Parser.Project;

namespace RunJit.Cli.RunJit.Generate.CustomEndpoint
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
        internal void EmbedFile(ProjectFile projectFile, string file)
        {
            // Check if seettings are active for  **\*.sql -> All SQLs are automatic embedded -> makes it much more easier
            var xdocument = XDocument.Load(projectFile.ProjectFileInfo.Value.FullName);
            var fileExtension = file.Split('.').Last();

            var itemGroup = new XElement("ItemGroup");
            var sqlElement = xdocument.Descendants().FirstOrDefault(e => e.Name.LocalName == "EmbeddedResource" && (e.Attribute("Include")?.Value == $@"**\*.{fileExtension}"));
            if (sqlElement.IsNull())
            {
                var embeddedResourceSql = new XElement("EmbeddedResource");
                embeddedResourceSql.Add(new XAttribute("Include", $@"**\*.{fileExtension}"));
                itemGroup.Add(embeddedResourceSql);
                xdocument.Root!.Add(itemGroup);
                
                xdocument.Save(projectFile.ProjectFileInfo.Value.FullName);
            }
        }
    }
}
