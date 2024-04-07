namespace RunJit.Cli.RunJit.Generate.Client
{
    internal interface IBuildClientGenerator
    {
        Client BuildFrom(ClientParameters clientGenParameters);

        bool IsThisBuilderFor(ClientParameters clientGenParameters);
    }
}
