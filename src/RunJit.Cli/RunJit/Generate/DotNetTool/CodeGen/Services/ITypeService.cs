namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal interface ITypeService
    {
        string GetFullQualifiedName(string projectName,
                                    FileInfo fileInfo);
    }
}
