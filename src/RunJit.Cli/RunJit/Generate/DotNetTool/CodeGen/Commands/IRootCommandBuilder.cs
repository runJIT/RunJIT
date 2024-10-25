namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal interface IRootCommandBuilder
    {
        string Build(string project,
                     CommandInfo parameterInfo,
                     string nameSpace);
    }
}
