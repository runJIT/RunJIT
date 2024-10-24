namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal record SubCommand
    {
        public List<SubCommand> SubCommands { get; init; } = new List<SubCommand>();

        public List<OptionInfo> Options { get; init; } = new List<OptionInfo>();

        public ArgumentInfo? Argument { get; init; } = null;

        public string Description { get; init; } = string.Empty;

        public string Value { get; init; } = string.Empty;

        public string Name { get; init; } = string.Empty;
    }
}
