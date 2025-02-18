using System.Xml.Linq;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;

namespace RunJit.Cli.Generate.DotNetTool
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
                                  XDocument projectDocument,
                                  DotNetToolInfos dotNetToolInfos)
        {
            // 1. Create a new PropertyGroup for .NET tool settings
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
                                                 new XElement("IsPublishable", "false"),
                                                 new XElement("ImplicitUsings", "enable"),
                                                 new XElement("ToolCommandName", dotNetToolInfos.NormalizedName.ToLower()));

            // 2. Add the comment and new PropertyGroup to the root of the project file
            projectDocument.Root!.Add(toolSettingsComment, toolPropertyGroup);

            // 3. Print success message
            consoleService.WriteSuccess($"Successfully modified {projectFileInfo.FullName} with .Net tool specific settings");

            return Task.CompletedTask;
        }
    }
}
