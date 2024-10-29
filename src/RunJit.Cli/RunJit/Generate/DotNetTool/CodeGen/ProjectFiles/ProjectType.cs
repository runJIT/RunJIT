using System.Xml.Linq;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal static class AddProjectTypeCodeGenExtension
    {
        internal static void AddProjectTypeCodeGen(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<INetToolCodeGen, ProjectTypeCodeGen>();
        }
    }

    internal sealed class ProjectTypeCodeGen(ConsoleService consoleService) : INetToolCodeGen
    {
        public Task GenerateAsync(FileInfo projectFileInfo,
                                  DotNetToolInfos dotNetToolInfos)
        {
            // 1. Load csproj file
            var projectFile = XDocument.Load(projectFileInfo.FullName);

            // 2. We have to go sure that the project file is a web project
            //    cause we are using http stuff and more. And all nugets from
            //    microsoft was set to deprecated so we have to use the correct
            //    way over the project type instead of using the nuget package
            //   <Project Sdk="Microsoft.NET.Sdk.Web">
            projectFile.Root!.Attribute("Sdk")!.Value = "Microsoft.NET.Sdk.Web";

            // 4. Save the changes back to the .csproj file
            projectFile.Save(projectFileInfo.FullName);

            // 5. Print success message
            consoleService.WriteSuccess($"Successfully modified {projectFileInfo.FullName} with .Net tool specific settings");

            return Task.CompletedTask;
        }
    }
}
