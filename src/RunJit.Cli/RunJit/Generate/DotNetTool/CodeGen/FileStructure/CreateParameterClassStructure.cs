﻿using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal static class AddCreateParameterClassStructureExtension
    {
        internal static void AddCreateParameterClassStructure(this IServiceCollection services)
        {
            services.AddParameterClassBuilder();

            services.AddSingletonIfNotExists<IBuildCommandFileStructure, CreateParameterClassStructure>();
        }
    }

    internal sealed class CreateParameterClassStructure(ParameterClassBuilder parameterClassBuilder) : IBuildCommandFileStructure
    {
        public void Create(string projectName,
                           CommandInfo? parentCommandInfo,
                           CommandTypeCollector commandTypeCollector,
                           string currentPath,
                           NameSpaceCollector namespaceCollector,
                           DirectoryInfo subCommnandDirectoryInfo,
                           CommandInfo commandInfo,
                           DotNetToolName dotNetToolName)

        {
            if (!commandInfo.Argument.IsNotNull() && !commandInfo.Options.Any() && !commandInfo.SubCommands.IsNullOrEmpty())
            {
                return;
            }

            var parameterModelClass = parameterClassBuilder.Build(projectName, commandInfo, currentPath);
            var parameterClassFileInfo = new FileInfo(Path.Combine(subCommnandDirectoryInfo.FullName, $"{commandInfo.NormalizedName}Parameters.cs"));
            File.WriteAllText(parameterClassFileInfo.FullName, parameterModelClass);
            namespaceCollector.Add($"{currentPath}.Service");
        }
    }
}
