namespace RunJit.Cli.RunJit.Generate.Client
{
    internal record ClientParameters(bool UseVisualStudio,
                                     bool Build,
                                     FileInfo SolutionFile);
}
