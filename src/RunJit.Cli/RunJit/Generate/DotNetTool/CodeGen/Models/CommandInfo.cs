using System.Diagnostics;
using System.Runtime.CompilerServices;
using RunJit.Cli.Services.Endpoints;
[assembly: InternalsVisibleTo("DotNetTool.Builder.Test")]

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    [DebuggerDisplay("{" + nameof(Name) + "}")]
    public record CommandInfo
    {
        public List<CommandInfo> SubCommands { get; init; } = new List<CommandInfo>();

        public List<OptionInfo> Options { get; init; } = new List<OptionInfo>();

        public ArgumentInfo? Argument { get; init; }

        public string Description { get; init; } = string.Empty;

        public string Value { get; init; } = string.Empty;

        public string Name { get; init; } = string.Empty;

        public string NormalizedName { get; init; } = string.Empty;

        public EndpointInfo? EndpointInfo { get; init; }

        public string CodeTemplate { get; init; } = string.Empty;

        public bool NoSyntaxTreeFormatting { get; init; }
    }
}
