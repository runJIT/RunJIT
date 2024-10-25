namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal interface ICommandServiceInterfaceBuilder
    {
        string Build(string project,
                     CommandInfo parameterInfo,
                     string nameSpace);
    }
}
