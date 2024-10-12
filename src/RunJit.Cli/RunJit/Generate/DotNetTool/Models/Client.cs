using System.Diagnostics;
using RunJit.Cli.Models;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    [DebuggerDisplay("Project: {" + nameof(ProjectName) + "} ToolName: {" + nameof(Cli.Models.DotNetToolName.Name) + "}")]
    public record DotNetTool(string ProjectName,
                             DotNetToolName DotNetToolName,
                             FileInfo SolutionFileInfo,
                             FileSystemInfo TargetDirectory);
}
