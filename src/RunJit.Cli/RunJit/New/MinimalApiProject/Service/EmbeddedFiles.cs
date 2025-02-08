using System.Xml.Linq;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;

namespace RunJit.Cli.New.MinimalApiProject
{
    internal static class AddEmbeddedFilesExtension
    {
        internal static void AddEmbeddedFiles(this IServiceCollection services)
        {
            services.AddRetryHelper();

            services.AddSingletonIfNotExists<IMinimalApiProjectSpecificCodeGen, EmbeddedFiles>();
            services.AddSingletonIfNotExists<IMinimalApiProjectTestSpecificCodeGen, EmbeddedFiles>();
        }
    }

    
    internal sealed class EmbeddedFiles(ConsoleService consoleService) : IMinimalApiProjectSpecificCodeGen,
                                                                         IMinimalApiProjectTestSpecificCodeGen
    {
        public Task GenerateAsync(FileInfo projectFileInfo,
                                  XDocument projectDocument,
                                  MinimalApiProjectInfos minimalApiProjectInfos)
        {
            // 1. Create a new item group for embedded files
            //    <ItemGroup>
            //        <EmbeddedResource Include="**\*.json" Exclude="bin\**\*;obj\**\*" />
            //    </ItemGroup>
            var toolEmbeddedFilesComment = new XComment("Embedded files area");

            // 2. Add wildcards for files which should be embedded
            var itemGroup = new XElement("ItemGroup");
            var embeddedResource = new XElement("EmbeddedResource");
            var includeAttribute = new XAttribute("Include", @"**\*.json");
            var excludeAttribute = new XAttribute("Exclude", @"bin\**\*;obj\**\*");

            embeddedResource.Add(includeAttribute);
            embeddedResource.Add(excludeAttribute);
            itemGroup.Add(embeddedResource);

            // 3. Add the comment and new PropertyGroup to the root of the project file
            projectDocument.Root!.Add(toolEmbeddedFilesComment, itemGroup);

            // 4. Print success message
            consoleService.WriteSuccess($"Successfully modified {projectFileInfo.FullName} with .Net tool specific settings");

            return Task.CompletedTask;
        }
    }
}
