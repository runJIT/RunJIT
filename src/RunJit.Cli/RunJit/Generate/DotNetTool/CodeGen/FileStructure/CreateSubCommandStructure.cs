using Extensions.Pack;
using RunJit.Cli.RunJit.Generate.DotNetTool.CodeGen.Commands;
using RunJit.Cli.RunJit.Generate.DotNetTool.CodeGen.Models;

namespace RunJit.Cli.RunJit.Generate.DotNetTool.CodeGen.FileStructure
{
    internal sealed class CreateSubCommandStructure(ICommandBuilderForSubCommands commandBuilderForSubCommands,
                                                    ISubCommandInterfaceBuilder subCommandInterfaceBuilder) : IBuildCommandFileStructure
    {
        public void Create(string projectName, CommandInfo parameter, CommandTypeCollector commandTypeCollector, string currentPath, NameSpaceCollector namespaceCollector, DirectoryInfo subCommnandDirectoryInfo, CommandInfo subCommand)
        {
            if (!subCommand.SubCommands.IsNotNull() || !subCommand.SubCommands.Any())
            {
                return;
            }

            var result = commandBuilderForSubCommands.Build(projectName, subCommand, parameter, currentPath);
            var filePath0 = new FileInfo(Path.Combine(subCommnandDirectoryInfo.FullName, $"{subCommand.NormalizedName}CommandBuilder.cs"));

            File.WriteAllText(filePath0.FullName, result);

            var subCommandBuilder = subCommandInterfaceBuilder.Build(projectName, subCommand, parameter, currentPath);
            var filePath1 = new FileInfo(Path.Combine(subCommnandDirectoryInfo.FullName, $"I{subCommand.NormalizedName}SubCommandBuilder.cs"));

            File.WriteAllText(filePath1.FullName, subCommandBuilder);

            var splittedNamespace = currentPath.Split(".").ToList();
            splittedNamespace.Remove(splittedNamespace.Last());
            var newNamespaceForInterface = splittedNamespace.Flatten(".");
            var interfaceType = $"{newNamespaceForInterface}.I{parameter.NormalizedName}SubCommandBuilder";
            var implementation = $"{currentPath}.{filePath0.NameWithoutExtension}";

            commandTypeCollector.Add(subCommand, new TypeToRegister(interfaceType, implementation));
        }
    }
}
