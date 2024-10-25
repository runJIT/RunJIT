namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal interface ICommandHandlerStringBuilder
    {
        string Build(CommandInfo parameterInfo);
        bool IsThisBuilderFor(CommandInfo parameterInfo);
    }
}
