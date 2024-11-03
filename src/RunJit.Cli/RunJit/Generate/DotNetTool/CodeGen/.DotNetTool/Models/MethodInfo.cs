using System.Diagnostics;

namespace RunJit.Cli.Generate.DotNetTool.Models
{
    [DebuggerDisplay("{" + nameof(MethodName) + "}")]
    internal sealed class MethodInfo(string methodName,
                                     string methodSyntax)
    {
        public string MethodName { get; } = methodName;

        public string MethodSyntax { get; } = methodSyntax;
    }
}
