using System.Xml.Linq;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal static class AddProjectSettingsCodeGenExtension
    {
        internal static void AddProjectSettingsCodeGen(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<IDotNetToolSpecificCodeGen, ProjectSettingsCodeGen>();
        }
    }

    internal sealed class ProjectSettingsCodeGen(ConsoleService consoleService) : IDotNetToolSpecificCodeGen
    {
        public Task GenerateAsync(FileInfo projectFileInfo,
                                  Models.DotNetToolInfos dotNetToolInfos)
        {
            // 1. Load csproj file
            var projectFile = XDocument.Load(projectFileInfo.FullName);

            // 2. Create a new PropertyGroup for .NET tool settings
            //    <PropertyGroup>
            //        <OutputType>Exe</OutputType>
            //        <PackAsTool>true</PackAsTool>
            //        <IsPackable>true</IsPackable>
            //        <ToolCommandName>mytool</ToolCommandName>
            //        <ImplicitUsings>enable</ImplicitUsings>
            //    </PropertyGroup>
            var toolSettingsComment = new XComment(".NET tool specific settings");
            var toolPropertyGroup = new XElement("PropertyGroup",
                                                 new XElement("OutputType", "Exe"),
                                                 new XElement("PackAsTool", "true"),
                                                 new XElement("IsPackable", "true"),
                                                 new XElement("ImplicitUsings", "enable"),
                                                 new XElement("ToolCommandName", dotNetToolInfos.NormalizedName.ToLower()));

            // 3. Add the comment and new PropertyGroup to the root of the project file
            projectFile.Root!.Add(toolSettingsComment, toolPropertyGroup);

            // 4. Save the changes back to the .csproj file
            projectFile.Save(projectFileInfo.FullName);

            // 5. Print success message
            consoleService.WriteSuccess($"Successfully modified {projectFileInfo.FullName} with .Net tool specific settings");

            return Task.CompletedTask;
        }
    }
}
