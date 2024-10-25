namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal interface IParameterClassBuilder
    {
        string Build(string projectName,
                     CommandInfo parameterInfo,
                     string nameSpace);
    }
}
