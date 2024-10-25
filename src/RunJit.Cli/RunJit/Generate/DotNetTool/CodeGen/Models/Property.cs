using System.Diagnostics;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    [DebuggerDisplay("{" + nameof(Name) + "}")]
    internal sealed class Property(string type,
                                   string name)
    {
        public string Type { get; } = type;

        public string Name { get; } = name;
    }
}
