namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal record OptionInfoInfo
    {
        public string Alias { get; } = string.Empty;

        public string Description { get; } = string.Empty;

        public bool IsIsRequired { get; }

        public ArgumentInfo? Argument { get; set; } = null;
    }
}
