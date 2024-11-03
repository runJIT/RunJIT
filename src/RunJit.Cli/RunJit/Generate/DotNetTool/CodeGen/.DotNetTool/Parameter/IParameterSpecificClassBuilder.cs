namespace RunJit.Cli.Generate.DotNetTool
{
    internal interface IParameterSpecificClassBuilder
    {
        string Build(string projectName,
                     Models.CommandInfo parameterInfo,
                     string nameSpace);

        bool IsThisBuilderFor(Models.CommandInfo parameterInfo);
    }
}
