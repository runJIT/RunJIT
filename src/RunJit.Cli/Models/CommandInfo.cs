using System.Collections.Immutable;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

[assembly: InternalsVisibleTo("DotNetTool.Builder.Test")]

namespace RunJit.Cli.Models
{
    [DebuggerDisplay("{" + nameof(Name) + "}")]
    [method: JsonConstructor]
    public class CommandInfo(
        string value,
        string name,
        string normalizedName,
        string description,
        ArgumentInfo? argumentInfo,
        IImmutableList<OptionInfo> options,
        IImmutableList<CommandInfo> subCommands)
        : InfoBase(value, name, normalizedName)
    {
        public IImmutableList<CommandInfo> SubCommands { get; } = subCommands;

        public IImmutableList<OptionInfo> Options { get; } = options;

        public ArgumentInfo? Argument { get; set; } = argumentInfo;

        public string Description { get; } = description;
    }
}
