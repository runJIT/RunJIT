namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal interface IConstructorArgumentBuilder
    {
        IEnumerable<CtorArgument> Build(CommandInfo parameterInfo);
    }
}
