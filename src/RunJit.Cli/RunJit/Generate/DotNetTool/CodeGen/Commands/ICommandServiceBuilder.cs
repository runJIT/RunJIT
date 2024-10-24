using RunJit.Cli.RunJit.Generate.DotNetTool.CodeGen.Models;

namespace RunJit.Cli.RunJit.Generate.DotNetTool.CodeGen.Commands
{
    internal interface ICommandServiceBuilder
    {
        string Build(string project, CommandInfo parameterInfo, string nameSpace);
    }
}
