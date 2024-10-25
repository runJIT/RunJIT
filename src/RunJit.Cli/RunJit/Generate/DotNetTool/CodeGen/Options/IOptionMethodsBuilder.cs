namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal interface IOptionMethodsBuilder
    {
        IEnumerable<MethodInfo> Build(IEnumerable<OptionInfo> options);
    }
}
