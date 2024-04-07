using System.CommandLine;

namespace RunJit.Cli.RunJit.New
{
    internal interface INewSubCommandBuilder
    {
        Command Build();
    }
}
