namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal interface INewOptionExpressionBuilder
    {
        string Build(Models.OptionInfo optionInfo);

        bool IsBuilderFor(Models.OptionInfo optionInfo);
    }
}
