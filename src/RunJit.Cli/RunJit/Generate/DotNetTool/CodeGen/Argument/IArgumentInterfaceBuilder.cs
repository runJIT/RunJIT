namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal interface IArgumentInterfaceBuilder
    {
        string Build(string projectName,
                     CommandInfo parameterInfo,
                     string nameSpace);
    }
}
