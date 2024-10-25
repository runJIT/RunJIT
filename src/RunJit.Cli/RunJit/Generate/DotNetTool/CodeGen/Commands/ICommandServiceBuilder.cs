namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal interface ICommandServiceBuilder
    {
        string Build(string project,
                     CommandInfo parameterInfo,
                     string nameSpace);
    }
}
