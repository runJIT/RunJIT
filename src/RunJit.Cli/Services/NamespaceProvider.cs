using System.Xml.Linq;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using Solution.Parser.Project;

namespace RunJit.Cli.Services
{
    public static class AddNamespaceProviderExtension
    {
        public static void AddNamespaceProvider(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<NamespaceProvider>();
        }
    }

    internal class NamespaceProvider
    {
        internal void SetNamespaceProviderAsync(ProjectFile projectFile,
                                                string @namespace,
                                                bool value)
        {
            var normalizedNamespace = @namespace.Replace($"{projectFile.ProjectFileInfo.FileNameWithoutExtenion}.", string.Empty);

            // runjit_005Cgenerate_005Cclient_005Cbuilders
            var resharperIgnoreEntry = normalizedNamespace.Split('.').Select(part => part.ToLower()).Flatten("_005C");

            var xDocument = GetXDocument(projectFile);

            if (xDocument.ToString().Contains(@namespace))
            {
                return;
            }

            XNamespace sNamespace = "clr-namespace:System;assembly=mscorlib";
            XNamespace xNamespace = "http://schemas.microsoft.com/winfx/2006/xaml";
            XElement element = new XElement(sNamespace + "Boolean");
            element.SetAttributeValue(xNamespace + "Key", $"/Default/CodeInspection/NamespaceProvider/NamespaceFoldersToSkip/={resharperIgnoreEntry}/@EntryIndexedValue");
            element.Value = value.ToString();
            xDocument.XDocument.Root!.Add(element);

            xDocument.XDocument.Save(xDocument.Path);
        }

        private (XDocument XDocument, string Path) GetXDocument(ProjectFile projectFile)
        {
            var dotSetttings = projectFile.ProjectFileInfo.Value.Directory!.EnumerateFiles($"{projectFile.ProjectFileInfo.Value.Name}.DotSettings").FirstOrDefault();

            if (dotSetttings.IsNotNull())
            {
                return (XDocument.Load(dotSetttings.FullName), dotSetttings.FullName);
            }

            var path = $"{projectFile.ProjectFileInfo.Value.FullName}.DotSettings";
            var dotSettingsTemplate = GetType().Assembly.GetEmbeddedFileAsStream("Generate.RestControllerNew.Code.NamespaceProvider.Template.DotSettings.xml");

            return (XDocument.Load(dotSettingsTemplate), path);
        }
    }
}
