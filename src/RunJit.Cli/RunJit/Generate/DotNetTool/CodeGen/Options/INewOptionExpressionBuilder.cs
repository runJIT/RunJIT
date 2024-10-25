namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal interface INewOptionExpressionBuilder
    {
        string Build(OptionInfo optionInfo);

        bool IsBuilderFor(OptionInfo optionInfo);
    }
}
