using RunJit.Cli.RunJit.Generate.DotNetTool.CodeGen.Models;

namespace RunJit.Cli.RunJit.Generate.DotNetTool.CodeGen.Commands
{
    internal interface IRootCommandBuilder
    {
        string Build(string project, CommandInfo parameterInfo, string nameSpace);
    }
}
