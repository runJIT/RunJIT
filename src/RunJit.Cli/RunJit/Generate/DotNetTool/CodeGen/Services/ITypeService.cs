namespace RunJit.Cli.RunJit.Generate.DotNetTool.CodeGen
{
    internal interface ITypeService
    {
        string GetFullQualifiedName(string projectName, FileInfo fileInfo);
    }
}
