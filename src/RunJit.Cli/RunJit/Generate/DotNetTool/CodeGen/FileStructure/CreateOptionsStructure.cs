using Extensions.Pack;
using RunJit.Cli.RunJit.Generate.DotNetTool.CodeGen.Models;
using RunJit.Cli.RunJit.Generate.DotNetTool.CodeGen.Options;

namespace RunJit.Cli.RunJit.Generate.DotNetTool.CodeGen.FileStructure
{
    internal sealed class CreateOptionsStructure(IOptionInterfaceBuilder optionInterfaceBuilder,
                                                 IOptionImplementationBuilder optionImplementationBuilder,
                                                 ITypeService typeService)
        : IBuildCommandFileStructure
    {
        public void Create(string projectName,
                           CommandInfo parameter,
                           CommandTypeCollector commandTypeCollector,
                           string currentPath,
                           NameSpaceCollector namespaceCollector,
                           DirectoryInfo subCommnandDirectoryInfo,
                           CommandInfo subCommand)
        {
            if (subCommand.Options.IsNullOrEmpty())
            {
                return;
            }

            var optionFolderPath = Path.Combine(subCommnandDirectoryInfo.FullName, "Options");
            var optionFolder = Directory.CreateDirectory(optionFolderPath);

            var optionsInterfaceSyntaxTree = optionInterfaceBuilder.Build(projectName, subCommand, currentPath);
            var optionsInterfaceFilePath = new FileInfo(Path.Combine(optionFolder.FullName, $"I{subCommand.NormalizedName}OptionsBuilder.cs"));

            File.WriteAllText(optionsInterfaceFilePath.FullName, optionsInterfaceSyntaxTree);

            var optionsImplementationSyntaxTree = optionImplementationBuilder.Build(projectName, subCommand, currentPath);
            var optionsImplementationFilePath = new FileInfo(Path.Combine(optionFolder.FullName, $"{subCommand.NormalizedName}OptionsBuilder.cs"));
            File.WriteAllText(optionsImplementationFilePath.FullName, optionsImplementationSyntaxTree);

            var interfaceToRegister = typeService.GetFullQualifiedName(projectName, optionsInterfaceFilePath);
            var implementationToRegister = typeService.GetFullQualifiedName(projectName, optionsImplementationFilePath);

            commandTypeCollector.Add(subCommand, new TypeToRegister(interfaceToRegister, implementationToRegister));

            namespaceCollector.Add($"{currentPath}.Options");
        }
    }
}
