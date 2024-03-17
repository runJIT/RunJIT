using System.CommandLine;

namespace RunJit.Cli.RunJit.Update
{
    internal interface IUpdateSubCommandBuilder
    {
        Command Build();
    }
}
