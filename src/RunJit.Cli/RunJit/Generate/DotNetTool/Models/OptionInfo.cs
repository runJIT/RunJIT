namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal record OptionInfo
    {
        public string Alias { get; init; } = string.Empty;

        public string Description { get; init; } = string.Empty;

        public bool IsRequired { get; init; }

        public ArgumentInfo? Argument { get; init; } = null;

        public string ArgumentInfoName { get; init; } = string.Empty;

        public string Value { get; init; } = string.Empty;

        public string Name { get; init; } = string.Empty;
    }
}
