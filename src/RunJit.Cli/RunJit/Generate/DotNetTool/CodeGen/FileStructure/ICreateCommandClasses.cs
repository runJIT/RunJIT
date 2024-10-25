namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal interface ICreateCommandClasses
    {
        void Invoke(string projectName,
                    CommandInfo parameter,
                    DirectoryInfo rootDirectory,
                    CommandTypeCollector commandTypeCollector,
                    string currentPath,
                    NameSpaceCollector namespaceCollector);
    }
}
