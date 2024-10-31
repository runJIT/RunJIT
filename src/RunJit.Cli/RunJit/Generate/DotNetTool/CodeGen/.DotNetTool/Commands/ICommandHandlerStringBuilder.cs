namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal interface ICommandHandlerStringBuilder
    {
        string Build(Models.CommandInfo parameterInfo);

        bool IsThisBuilderFor(Models.CommandInfo parameterInfo);
    }
}
