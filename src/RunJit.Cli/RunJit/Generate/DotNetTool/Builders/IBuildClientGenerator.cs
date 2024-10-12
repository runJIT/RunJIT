namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal interface IBuildDotNetToolGenerator
    {
        DotNetTool BuildFrom(DotNetToolParameters clientGenParameters);

        bool IsThisBuilderFor(DotNetToolParameters clientGenParameters);
    }
}
