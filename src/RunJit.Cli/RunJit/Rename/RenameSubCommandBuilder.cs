using System.CommandLine;

namespace RunJit.Cli.RunJit.Rename
{
    internal interface IRenameSubCommandBuilder
    {
        Command Build();
    }
}
