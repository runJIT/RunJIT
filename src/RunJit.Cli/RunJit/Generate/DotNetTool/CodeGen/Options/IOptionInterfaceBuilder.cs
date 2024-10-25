namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal interface IOptionInterfaceBuilder
    {
        string Build(string projectName,
                     CommandInfo parameterInfo,
                     string nameSpace);
    }
}
