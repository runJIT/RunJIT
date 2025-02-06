using System.Xml.Linq;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Generate.DotNetTool;
using RunJit.Cli.Services;

namespace RunJit.Cli.New.MinimalApiProject.CodeGen.MinimalApiProject.Test.ProjectFiles
{
    internal static class AddProjectSettingsCodeGenExtension
    {
        internal static void AddProjectSettingsCodeGen(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<IMinimalApiProjectTestSpecificCodeGen, ProjectSettingsCodeGen>();
        }
    }

    internal sealed class ProjectSettingsCodeGen(ConsoleService consoleService) : IMinimalApiProjectTestSpecificCodeGen
    {
        public Task GenerateAsync(FileInfo projectFileInfo,
                                  XDocument projectDocument,
                                  MinimalApiProjectInfos minimalApiProjectInfos)
        {
            // 1. Create a new PropertyGroup for .NET tool settings
            //    <PropertyGroup>
            //        <IsPackable>false</IsPackable>
            //        <ImplicitUsings>enable</ImplicitUsings>
            //    </PropertyGroup>
            var toolSettingsComment = new XComment(".NET tool specific settings");

            var toolPropertyGroup = new XElement("PropertyGroup",
                                                 new XElement("IsPackable", "false"));

            // 2. Add the comment and new PropertyGroup to the root of the project file
            projectDocument.Root!.Add(toolSettingsComment, toolPropertyGroup);

            // 3. Print success message
            consoleService.WriteSuccess($"Successfully modified {projectFileInfo.FullName} with .Net tool specific settings");

            return Task.CompletedTask;
        }
    }
}
