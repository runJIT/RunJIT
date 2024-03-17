using System.Diagnostics;

namespace RunJit.Cli.Models
{
    [DebuggerDisplay("{" + nameof(Name) + "}")]
    internal class Property(string type, string name)
    {
        public string Type { get; } = type;
        public string Name { get; } = name;
    }
}
