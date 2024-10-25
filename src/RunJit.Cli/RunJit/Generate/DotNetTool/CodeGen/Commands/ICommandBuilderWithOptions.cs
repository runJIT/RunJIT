namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal interface ICommandBuilderWithOptions
    {
        string Build(string project,
                     CommandInfo parameterInfo,
                     CommandInfo parent,
                     string nameSpace);
    }
}
