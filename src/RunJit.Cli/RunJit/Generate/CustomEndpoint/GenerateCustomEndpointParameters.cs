namespace RunJit.Cli.RunJit.Generate.CustomEndpoint
{
    internal record GenerateCustomEndpointParameters(DirectoryInfo TargetFolder,
                                                     string EndpointData,
                                                     bool OverwriteCode);
}
