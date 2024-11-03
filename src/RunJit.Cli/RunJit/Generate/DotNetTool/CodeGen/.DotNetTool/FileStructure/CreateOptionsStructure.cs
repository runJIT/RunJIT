﻿using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Generate.DotNetTool.Models;

namespace RunJit.Cli.Generate.DotNetTool
{
    internal static class AddCreateOptionsStructureExtension
    {
        internal static void AddCreateOptionsStructure(this IServiceCollection services)
        {
            services.AddOptionImplementationBuilder();
            services.AddTypeService();

            services.AddSingletonIfNotExists<IBuildCommandFileStructure, CreateOptionsStructure>();
        }
    }

    internal sealed class CreateOptionsStructure(OptionImplementationBuilder optionImplementationBuilder,
                                                 TypeService typeService)
        : IBuildCommandFileStructure
    {
        public void Create(string projectName,
                           CommandInfo? parentCommandInfo,
                           CommandTypeCollector commandTypeCollector,
                           string currentPath,
                           NameSpaceCollector namespaceCollector,
                           DirectoryInfo subCommnandDirectoryInfo,
                           CommandInfo commandInfo,
                           DotNetToolInfos dotNetToolName)
        {
            if (commandInfo.Options.IsNullOrEmpty())
            {
                return;
            }

            var optionFolderPath = Path.Combine(subCommnandDirectoryInfo.FullName, "Options");
            var optionFolder = Directory.CreateDirectory(optionFolderPath);

            var optionsImplementationSyntaxTree = optionImplementationBuilder.Build(projectName, commandInfo, currentPath);
            var optionsImplementationFilePath = new FileInfo(Path.Combine(optionFolder.FullName, $"{commandInfo.NormalizedName}OptionsBuilder.cs"));
            File.WriteAllText(optionsImplementationFilePath.FullName, optionsImplementationSyntaxTree);

            var implementationToRegister = typeService.GetFullQualifiedName(projectName, optionsImplementationFilePath);

            commandTypeCollector.Add(commandInfo, new Models.TypeToRegister(implementationToRegister, implementationToRegister));

            namespaceCollector.Add($"{currentPath}.Options");
        }
    }
}
