namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal interface IRootCommandInterfaceBuilder
    {
        string Build(string project,
                     CommandInfo parameterInfo,
                     string nameSpace);
    }
}
