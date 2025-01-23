using RunJit.Cli.Generate.DotNetTool.Models;

namespace RunJit.Cli.Generate.DotNetTool
{
    internal interface INewOptionExpressionBuilder
    {
        string Build(OptionInfo optionInfo);

        bool IsBuilderFor(OptionInfo optionInfo);
    }
}
