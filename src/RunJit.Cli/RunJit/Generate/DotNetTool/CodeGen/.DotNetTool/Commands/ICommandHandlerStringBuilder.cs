using RunJit.Cli.Generate.DotNetTool.Models;

namespace RunJit.Cli.Generate.DotNetTool
{
    internal interface ICommandHandlerStringBuilder
    {
        string Build(CommandInfo parameterInfo);

        bool IsThisBuilderFor(CommandInfo parameterInfo);
    }
}
