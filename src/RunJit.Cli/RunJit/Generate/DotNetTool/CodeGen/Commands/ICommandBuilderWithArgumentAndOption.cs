namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal interface ICommandBuilderWithArgumentAndOption
    {
        string Build(string project, CommandInfo parameterInfo, CommandInfo parent, string nameSpace);
    }
}
