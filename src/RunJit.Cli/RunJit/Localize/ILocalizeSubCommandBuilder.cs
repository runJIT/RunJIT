using System.CommandLine;

namespace RunJit.Cli.RunJit.Localize
{
    internal interface ILocalizeSubCommandBuilder
    {
        Command Build();
    }
}
