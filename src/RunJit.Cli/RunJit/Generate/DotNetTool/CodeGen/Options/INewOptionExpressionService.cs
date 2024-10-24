using RunJit.Cli.RunJit.Generate.DotNetTool.CodeGen.Models;

namespace RunJit.Cli.RunJit.Generate.DotNetTool.CodeGen.Options
{
    internal interface INewOptionExpressionService
    {
        string Build(OptionInfo optionInfo);
    }
}
