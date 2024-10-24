using RunJit.Cli.RunJit.Generate.DotNetTool.CodeGen.Models;

namespace RunJit.Cli.RunJit.Generate.DotNetTool.CodeGen.FileStructure
{
    internal interface IBuildCommandFileStructure
    {
        void Create(string projectName, CommandInfo parameter, CommandTypeCollector commandTypeCollector, string currentPath, NameSpaceCollector namespaceCollector, DirectoryInfo subCommnandDirectoryInfo, CommandInfo subCommand);
    }
}
