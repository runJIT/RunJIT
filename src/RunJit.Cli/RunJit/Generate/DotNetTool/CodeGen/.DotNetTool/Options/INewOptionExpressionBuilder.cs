namespace RunJit.Cli.Generate.DotNetTool
{
    internal interface INewOptionExpressionBuilder
    {
        string Build(Models.OptionInfo optionInfo);

        bool IsBuilderFor(Models.OptionInfo optionInfo);
    }
}
