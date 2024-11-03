namespace RunJit.Cli.Generate.DotNetTool
{
    internal interface ICommandHandlerStringBuilder
    {
        string Build(Models.CommandInfo parameterInfo);

        bool IsThisBuilderFor(Models.CommandInfo parameterInfo);
    }
}
