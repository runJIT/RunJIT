namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal interface IBuildCommandFileStructure
    {
        void Create(string projectName,
                    CommandInfo? parentCommandInfo,
                    CommandTypeCollector commandTypeCollector,
                    string currentPath,
                    NameSpaceCollector namespaceCollector,
                    DirectoryInfo subCommnandDirectoryInfo,
                    CommandInfo commandInfo);
    }
}
