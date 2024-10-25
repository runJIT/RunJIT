namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal interface ISubCommandInterfaceBuilder
    {
        string Build(string project, CommandInfo parameterInfo, CommandInfo parent, string nameSpace);
    }
}
