using System.Diagnostics;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
[assembly: InternalsVisibleTo("DotNetTool.Builder.Test")]

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    [DebuggerDisplay("{" + nameof(Name) + "}")]
    [method: JsonConstructor]
    public class CommandInfo(string value,
                             string name,
                             string normalizedName,
                             string description,
                             ArgumentInfo argumentInfo,
                             IEnumerable<OptionInfo> options,
                             IEnumerable<CommandInfo> subCommands)
        : InfoBase(value, name, normalizedName)
    {
        public IEnumerable<CommandInfo> SubCommands { get; } = subCommands;

        public IEnumerable<OptionInfo> Options { get; } = options;

        public ArgumentInfo Argument { get; set; } = argumentInfo;

        public string Description { get; } = description;
    }
}
