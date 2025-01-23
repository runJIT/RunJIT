using RunJit.Cli.Generate.DotNetTool.Models;

namespace RunJit.Cli.Generate.DotNetTool
{
    internal interface IParameterSpecificClassBuilder
    {
        string Build(string projectName,
                     CommandInfo parameterInfo,
                     string nameSpace);

        bool IsThisBuilderFor(CommandInfo parameterInfo);
    }
}
