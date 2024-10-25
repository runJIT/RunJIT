namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal interface IParameterSpecificClassBuilder
    {
        string Build(string projectName,
                     CommandInfo parameterInfo,
                     string nameSpace);

        bool IsThisBuilderFor(CommandInfo parameterInfo);
    }
}
