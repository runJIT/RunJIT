using RunJit.Cli.RunJit.Generate.DotNetTool.CodeGen.Models;

namespace RunJit.Cli.RunJit.Generate.DotNetTool.CodeGen.FileStructure
{
    internal interface ICreateCommandClasses
    {
        void Invoke(string projectName, CommandInfo parameter, DirectoryInfo rootDirectory,
            CommandTypeCollector commandTypeCollector, string currentPath, NameSpaceCollector namespaceCollector);
    }
}
