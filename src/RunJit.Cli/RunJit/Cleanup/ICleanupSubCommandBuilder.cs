using System.CommandLine;

namespace RunJit.Cli.RunJit.Cleanup
{
    internal interface ICleanupSubCommandBuilder
    {
        Command Build();
    }
}
