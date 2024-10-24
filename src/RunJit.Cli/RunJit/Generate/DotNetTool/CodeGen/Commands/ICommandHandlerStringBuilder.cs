using RunJit.Cli.RunJit.Generate.DotNetTool.CodeGen.Models;

namespace RunJit.Cli.RunJit.Generate.DotNetTool.CodeGen.Commands
{
    internal interface ICommandHandlerStringBuilder
    {
        string Build(CommandInfo parameterInfo);
        bool IsThisBuilderFor(CommandInfo parameterInfo);
    }
}
