using System.Diagnostics;

namespace RunJit.Cli.Generate.DotNetTool.Models
{
    [DebuggerDisplay("ExitCode: '{" + nameof(ExitCode) + "}'")]
    internal sealed class CliRunResult(int exitCode,
                                       string output)
    {
        public int ExitCode { get; } = exitCode;

        public string Output { get; } = output;
    }
}
