using System.Xml.Linq;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;
using Solution.Parser.Project;

namespace RunJit.Cli.Generate.DotNetTool.DotNetTool.Test
{
    internal static class AddProjectSettingsCodeGenExtension
    {
        internal static void AddProjectSettingsCodeGen(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<IDotNetToolTestSpecificCodeGen, ProjectSettingsCodeGen>();
        }
    }

    internal sealed class ProjectSettingsCodeGen(ConsoleService consoleService) : IDotNetToolTestSpecificCodeGen
    {
        public Task GenerateAsync(FileInfo projectFileInfo,
                                  XDocument projectDocument,
                                  DotNetToolInfos dotNetToolInfos,
                                  ProjectFile? webApiProject)
        {
            // 1. Create a new PropertyGroup for .NET tool settings
            //    <PropertyGroup>
            //        <IsPackable>false</IsPackable>
            //        <ImplicitUsings>enable</ImplicitUsings>
            //    </PropertyGroup>
            var toolSettingsComment = new XComment(".NET tool specific settings");

            var toolPropertyGroup = new XElement("PropertyGroup",
                                                 new XElement("IsPackable", "false"),
                                                 new XElement("ImplicitUsings", "enable"));

            // 2. Add the comment and new PropertyGroup to the root of the project file
            projectDocument.Root!.Add(toolSettingsComment, toolPropertyGroup);

            // 3. Print success message
            consoleService.WriteSuccess($"Successfully modified {projectFileInfo.FullName} with .Net tool specific settings");

            return Task.CompletedTask;
        }
    }
}
