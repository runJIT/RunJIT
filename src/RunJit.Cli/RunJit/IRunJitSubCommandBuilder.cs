using System.CommandLine;

namespace RunJit.Cli.RunJit
{
    internal interface IRunJitSubCommandBuilder
    {
        Command Build();
    }
}
