using System.Diagnostics;
using RunJit.Cli.Generate.DotNetTool.Models;

namespace RunJit.Cli.New.MinimalApiProject
{
    [DebuggerDisplay("Project: {" + nameof(ProjectName) + "} ToolName: {" + nameof(NormalizedName) + "}")]
    internal record MinimalApiProjectInfos
    {
        public required string ProjectName { get; init; }

        public required string BasePath { get; init; }

        public required string Name { get; init; }

        public required string NormalizedName { get; init; }

        public required string NetVersion { get; init; } = "net9.0";
    }
}
