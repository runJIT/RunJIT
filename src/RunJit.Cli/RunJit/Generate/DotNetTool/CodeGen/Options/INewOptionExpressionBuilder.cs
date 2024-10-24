using RunJit.Cli.RunJit.Generate.DotNetTool.CodeGen.Models;

namespace RunJit.Cli.RunJit.Generate.DotNetTool.CodeGen.Options
{
    internal interface INewOptionExpressionBuilder
    {
        string Build(OptionInfo optionInfo);
        bool IsBuilderFor(OptionInfo optionInfo);
    }
}
