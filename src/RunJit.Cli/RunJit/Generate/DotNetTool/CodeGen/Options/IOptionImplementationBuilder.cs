namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal interface IOptionImplementationBuilder
    {
        string Build(string projectName, CommandInfo parameterInfo, string nameSpace);
    }
}
