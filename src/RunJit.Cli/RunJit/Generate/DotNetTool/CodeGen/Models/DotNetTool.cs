using System.Diagnostics;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    [DebuggerDisplay("Project: {" + nameof(ProjectName) + "} ToolName: {" + nameof(Generate.DotNetTool.DotNetToolName.Name) + "}")]
    internal record DotNetToolInfos
    {
        public string ProjectName { get; init; } = string.Empty;

        public required DotNetToolName DotNetToolName { get; init; }

        public required CommandInfo CommandInfo { get; init; }
    }
}
