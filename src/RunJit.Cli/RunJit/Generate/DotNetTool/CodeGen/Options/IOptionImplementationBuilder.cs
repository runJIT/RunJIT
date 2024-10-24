using RunJit.Cli.RunJit.Generate.DotNetTool.CodeGen.Models;

namespace RunJit.Cli.RunJit.Generate.DotNetTool.CodeGen.Options
{
    internal interface IOptionImplementationBuilder
    {
        string Build(string projectName, CommandInfo parameterInfo, string nameSpace);
    }
}
