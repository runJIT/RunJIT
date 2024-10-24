using RunJit.Cli.RunJit.Generate.DotNetTool.CodeGen.Models;

namespace RunJit.Cli.RunJit.Generate.DotNetTool.CodeGen.Argument
{
    internal interface IArgumentBuilder
    {
        string Build(string projectName, CommandInfo parameterInfo, string nameSpace);
    }
}
