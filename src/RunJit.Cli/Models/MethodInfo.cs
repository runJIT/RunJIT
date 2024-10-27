using System.Diagnostics;

namespace RunJit.Cli.Models
{
    [DebuggerDisplay("{" + nameof(MethodName) + "}")]
    internal sealed class MethodInfo(string methodName,
                              string methodSyntax)
    {
        public string MethodName { get; } = methodName;

        public string MethodSyntax { get; } = methodSyntax;
    }
}
