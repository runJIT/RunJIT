using System.Xml.Linq;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;
using Solution.Parser.CSharp;
using StackExchange.Redis;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    public static class AddProjectEmbeddedFilesCodeGenExtension
    {
        public static void AddProjectEmbeddedFilesCodeGen(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<INetToolCodeGen, ProjectEmbeddedFilesCodeGen>();
        }
    }

    internal class ProjectEmbeddedFilesCodeGen(ConsoleService consoleService) : INetToolCodeGen
    {
        public Task GenerateAsync(FileInfo projectFileInfo,
                                  DotNetToolInfos dotNetToolInfos)
        {
            // 1. Load csproj file
            var projectFile = XDocument.Load(projectFileInfo.FullName);

            // 2. Create a new item group for embedded files
            //    <ItemGroup>
            //        <EmbeddedResource Include="**\*.json" Exclude="bin\**\*;obj\**\*" />
            //    </ItemGroup>
            var toolEmbeddedFilesComment = new XComment("Embedded files area");

            var itemGroup = new XElement("ItemGroup");
            var embeddedResource = new XElement("EmbeddedResource");
            var includeAttribute = new XAttribute("Include", $@"**\*.json");
            var excludeAttribute = new XAttribute("Exclude", $@"bin\**\*;obj\**\*");

            embeddedResource.Add(includeAttribute);
            embeddedResource.Add(excludeAttribute);
            itemGroup.Add(embeddedResource);

            // 3. Add the comment and new PropertyGroup to the root of the project file
            projectFile.Root!.Add(toolEmbeddedFilesComment, itemGroup);

            // 4. Save the changes back to the .csproj file
            projectFile.Save(projectFileInfo.FullName);
            
            // 5. Print success message
            consoleService.WriteSuccess($"Successfully modified {projectFileInfo.FullName} with .Net tool specific settings");

            return Task.CompletedTask;
        }
    }
}
