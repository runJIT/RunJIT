namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    public record OptionInfo
    {
        public string Alias { get; init; } = string.Empty;

        public string Description { get; init; } = string.Empty;

        public bool IsIsRequired { get; init; }

        public ArgumentInfo? Argument { get; init; }

        public string Value { get; init; } = string.Empty;

        public string Name { get; init; } = string.Empty;

        public string NormalizedName { get; init; } = string.Empty;
    }
}
