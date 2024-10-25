namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal interface IArgumentBuilder
    {
        string Build(string projectName, CommandInfo parameterInfo, string nameSpace);
    }
}
