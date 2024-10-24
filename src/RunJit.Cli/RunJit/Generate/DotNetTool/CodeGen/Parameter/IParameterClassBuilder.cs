using RunJit.Cli.RunJit.Generate.DotNetTool.CodeGen.Models;

namespace RunJit.Cli.RunJit.Generate.DotNetTool.CodeGen.Parameter
{
    internal interface IParameterClassBuilder
    {
        string Build(string projectName, CommandInfo parameterInfo, string nameSpace);
    }
}
