using System.CommandLine;

namespace RunJit.Cli.RunJit.Check.Backend
{
    internal interface ICheckBackendSubCommandBuilder
    {
        Command Build();
    }
}
