using System.Diagnostics;

namespace RunJit.Cli.RunJit.Generate.DotNetTool.CodeGen.Models
{
    [DebuggerDisplay("Project: {" + nameof(ProjectName) + "} ToolName: {" + nameof(Models.DotNetToolName.Name) + "}")]
    internal sealed class DotNetTool(string projectName,
                                     DotNetToolName dotNetToolName,
                                     CommandInfo parameterInfo)
    {
        public string ProjectName { get; } = projectName;

        public DotNetToolName DotNetToolName { get; } = dotNetToolName;

        public CommandInfo ParameterInfo { get; } = parameterInfo;
    }
}
