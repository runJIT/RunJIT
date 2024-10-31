using System.Diagnostics;

namespace RunJit.Cli.RunJit.Generate.DotNetTool.Models
{
    [DebuggerDisplay("{" + nameof(MethodName) + "}")]
    internal sealed class MethodInfo(string methodName,
                                     string methodSyntax)
    {
        public string MethodName { get; } = methodName;

        public string MethodSyntax { get; } = methodSyntax;
    }
}
