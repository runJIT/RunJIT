using RunJit.Cli.RunJit.Generate.DotNetTool.CodeGen.Models;

namespace RunJit.Cli.RunJit.Generate.DotNetTool.CodeGen.Parameter
{
    internal interface IParameterSpecificClassBuilder
    {
        string Build(string projectName, CommandInfo parameterInfo, string nameSpace);
        bool IsThisBuilderFor(CommandInfo parameterInfo);
    }
}
