namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal class ParameterInfo
    {
        public List<SubCommand> SubCommands { get; init; } = new List<SubCommand>();

        public List<OptionInfoInfo> Options { get; set; } = new List<OptionInfoInfo>();

        public ArgumentInfo? Argument { get; init; } = null;

        public string Description { get; set; } = string.Empty;

        public string Value { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
    }
}
