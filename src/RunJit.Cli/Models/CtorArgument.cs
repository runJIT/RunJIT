using System.Diagnostics;
using Extensions.Pack;

namespace RunJit.Cli.Models
{
    [DebuggerDisplay("{" + nameof(Name) + "}")]
    internal sealed class CtorArgument(string type,
                                string name)
    {
        public string Type { get; } = type;

        public string Name { get; } = name;

        public string NormalizedName { get; } = name.FirstCharToUpper();
    }
}
