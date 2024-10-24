using System.Diagnostics;

namespace RunJit.Cli.RunJit.Generate.DotNetTool.CodeGen.Models
{
    [DebuggerDisplay("{" + nameof(Value) + "}")]
    public abstract class InfoBase(string value,
                                   string name,
                                   string normalizedName)
    {
        public string Value { get; } = value;

        public string Name { get; } = name;

        public string NormalizedName { get; set; } = normalizedName;
    }
}
