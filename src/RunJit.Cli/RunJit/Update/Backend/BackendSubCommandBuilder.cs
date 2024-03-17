using System.CommandLine;

namespace RunJit.Cli.RunJit.Update.Backend
{
    internal interface IBackendSubCommandBuilder
    {
        Command Build();
    }
}
