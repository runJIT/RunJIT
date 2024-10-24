using RunJit.Cli.RunJit.Generate.DotNetTool.CodeGen.Models;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal record DotNetToolInfos
    {
        public string ProjectName { get; init; } = string.Empty;

        public required DotNetToolName DotNetToolName { get; init; }

        public required CommandInfo CommandInfo { get; init; }
    }
}
