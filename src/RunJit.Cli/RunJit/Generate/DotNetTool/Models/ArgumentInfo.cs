namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal record ArgumentInfo
        {
            public string Description { get; init; } = string.Empty;

            public string Type { get; init; } = string.Empty;

            public string Name { get; init; } = string.Empty;
        }
}
