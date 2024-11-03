using RunJit.Cli.Generate.DotNetTool.Models;

namespace RunJit.Cli.Generate.DotNetTool
{
    internal interface IBuildCommandFileStructure
    {
        void Create(string projectName,
                    CommandInfo? parentCommandInfo,
                    CommandTypeCollector commandTypeCollector,
                    string currentPath,
                    NameSpaceCollector namespaceCollector,
                    DirectoryInfo subCommnandDirectoryInfo,
                    CommandInfo commandInfo,
                    DotNetToolInfos dotNetToolName);
    }
}
