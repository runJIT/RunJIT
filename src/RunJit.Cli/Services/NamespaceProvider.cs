using System.Xml.Linq;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.Services
{
    internal static class AddNamespaceProviderExtension
    {
        internal static void AddNamespaceProvider(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<NamespaceProvider>();
        }
    }

    internal sealed class NamespaceProvider
    {
        private const string Template = """
                                        <wpf:ResourceDictionary xml:space="preserve" xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
                                                                xmlns:s="clr-namespace:System;assembly=mscorlib"
                                                                xmlns:ss="urn:shemas-jetbrains-com:settings-storage-xaml"
                                                                xmlns:wpf="http://schemas.microsoft.com/winfx/2006/xaml/presentation">
                                        </wpf:ResourceDictionary>
                                        """;

        internal void SetNamespaceProviderAsync(FileInfo projectFile,
                                                string @namespace,
                                                bool value)
        {
            var normalizedNamespace = @namespace.Replace($"{projectFile.NameWithoutExtension()}.", string.Empty);

            // runjit_005Cgenerate_005Cclient_005Cbuilders
            var resharperIgnoreEntry = normalizedNamespace.Split('.').Select(part => part.ToLower()).Flatten("_005C");

            var xDocument = GetXDocument(projectFile);

            if (xDocument.ToString().Contains($"/={normalizedNamespace.ToLower()}/"))
            {
                return;
            }

            XNamespace sNamespace = "clr-namespace:System;assembly=mscorlib";
            XNamespace xNamespace = "http://schemas.microsoft.com/winfx/2006/xaml";
            var element = new XElement(sNamespace + "Boolean");
            element.SetAttributeValue(xNamespace + "Key", $"/Default/CodeInspection/NamespaceProvider/NamespaceFoldersToSkip/={resharperIgnoreEntry}/@EntryIndexedValue");
            element.Value = value.ToString();
            xDocument.XDocument.Root!.Add(element);

            xDocument.XDocument.Save(xDocument.Path);
        }

        private (System.Xml.Linq.XDocument XDocument, string Path) GetXDocument(FileInfo projectFile)
        {
            var dotSetttings = projectFile.Directory!.EnumerateFiles($"{projectFile.Name}.DotSettings").FirstOrDefault();

            if (dotSetttings.IsNotNull())
            {
                return (XDocument.Load(dotSetttings.FullName), dotSetttings.FullName);
            }

            var path = $"{projectFile.FullName}.DotSettings";

            var xDocument = XDocument.Parse(Template);

            return (xDocument, path);
        }
    }
}
