using System.CommandLine;

namespace RunJit.Cli.RunJit.Check
{
    internal interface ICheckSubCommandBuilder
    {
        Command Build();
    }
}
