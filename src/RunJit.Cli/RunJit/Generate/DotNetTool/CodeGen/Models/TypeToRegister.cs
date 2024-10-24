using System.Diagnostics;

namespace RunJit.Cli.RunJit.Generate.DotNetTool.CodeGen.Models
{
    [DebuggerDisplay("{" + nameof(InterfaceType) + "} - " + "{" + nameof(ImplementationType) + "}")]
    internal sealed class TypeToRegister(string interfaceType,
                                         string implementationType)
    {
        public string InterfaceType { get; } = interfaceType;

        public string ImplementationType { get; } = implementationType;
    }
}
