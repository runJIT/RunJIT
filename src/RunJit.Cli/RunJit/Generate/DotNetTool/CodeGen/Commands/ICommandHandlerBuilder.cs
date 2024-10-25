namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal interface ICommandHandlerBuilder
    {
        string Build(CommandInfo parameterInfo);
    }
}
