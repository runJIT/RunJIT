namespace RunJit.Cli.Generate.DotNetTool
{
    internal record DotNetToolParameters(bool UseVisualStudio,
                                         bool Build,
                                         FileInfo SolutionFile,
                                         string ToolName);
}
