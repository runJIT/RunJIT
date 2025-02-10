using System.Diagnostics;
using RunJit.Cli.Generate.DotNetTool.Models;

namespace RunJit.Cli.Generate.DotNetTool
{
    [DebuggerDisplay("Project: {" + nameof(ProjectName) + "} ToolName: {" + nameof(NormalizedName) + "}")]
    internal record DotNetToolInfos
    {
        public required string ProjectName { get; init; }

        public required string Name { get; init; }

        public required string NormalizedName { get; init; }

        public required CommandInfo CommandInfo { get; init; }

        public required string NetVersion { get; init; } = "net9.0";
    }
}
