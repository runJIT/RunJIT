namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal record DotNetToolParameters(
        bool UseVisualStudio,
        bool Build,
        FileInfo SolutionFile);
}
