using System.Diagnostics;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    [DebuggerDisplay("ExitCode: '{" + nameof(ExitCode) + "}'")]
    internal sealed class CliRunResult(int exitCode,
                                       string output)
    {
        public int ExitCode { get; } = exitCode;

        public string Output { get; } = output;
    }
}
