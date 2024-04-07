using System.CommandLine;

namespace RunJit.Cli.RunJit.Generate
{
    internal interface IGenerateSubCommandBuilder
    {
        Command Build();
    }
}
