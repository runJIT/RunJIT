using System.CommandLine;

namespace RunJit.Cli.RunJit.Fix
{
    internal interface IFixSubCommandBuilder
    {
        Command Build();
    }
}
