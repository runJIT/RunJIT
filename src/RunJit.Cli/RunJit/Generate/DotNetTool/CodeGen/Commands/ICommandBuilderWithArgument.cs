namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal interface ICommandBuilderWithArgument
    {
        string Build(string project, CommandInfo parameterInfo, CommandInfo parent, string nameSpace);
    }
}
