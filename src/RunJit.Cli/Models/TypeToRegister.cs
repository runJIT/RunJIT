using System.Diagnostics;

namespace RunJit.Cli.Models
{
    [DebuggerDisplay("{" + nameof(InterfaceType) + "} - " + "{" + nameof(ImplementationType) + "}")]
    internal class TypeToRegister(string interfaceType, string implementationType)
    {
        public string InterfaceType { get; } = interfaceType;
        public string ImplementationType { get; } = implementationType;
    }
}
