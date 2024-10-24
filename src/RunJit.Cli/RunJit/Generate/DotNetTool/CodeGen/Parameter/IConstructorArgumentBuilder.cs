using RunJit.Cli.RunJit.Generate.DotNetTool.CodeGen.Models;

namespace RunJit.Cli.RunJit.Generate.DotNetTool.CodeGen.Parameter
{
    internal interface IConstructorArgumentBuilder
    {
        IEnumerable<CtorArgument> Build(CommandInfo parameterInfo);
    }
}
