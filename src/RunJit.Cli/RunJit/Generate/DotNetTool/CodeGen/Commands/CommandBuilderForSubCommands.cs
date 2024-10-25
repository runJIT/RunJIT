using Argument.Check;
using Extensions.Pack;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal sealed class CommandBuilderForSubCommands : ICommandBuilderForSubCommands
    {
        private const string Template =
@"using System.CommandLine;
using System.CommandLine.Invocation;  

namespace $namespace$
{    
    internal sealed class $command-name$CommandBuilder : I$parent-command-name$SubCommandBuilder
    {
        private readonly IEnumerable<I$command-name$SubCommandBuilder> _$command-argument-name$SubCommandBuilders;

        public $command-name$CommandBuilder(IEnumerable<I$command-name$SubCommandBuilder> $command-argument-name$SubCommandBuilders)
        {
            _$command-argument-name$SubCommandBuilders = $command-argument-name$SubCommandBuilders;
        }

        public Command Build()
        {
            var $command-argument-name$Command = new Command(""$command-argument-name$"", ""$command-description$"");
            _$command-argument-name$SubCommandBuilders.ToList().ForEach(builder => $command-argument-name$Command.AddCommand(builder.Build()));            
            return $command-argument-name$Command;
        }
    }
}";

        private readonly ICommandHandlerBuilder _commandHandlerBuilder;

        public CommandBuilderForSubCommands(ICommandHandlerBuilder commandHandlerBuilder)
        {
            Throw.IfNull(() => commandHandlerBuilder);

            _commandHandlerBuilder = commandHandlerBuilder;
        }

        public string Build(string project, CommandInfo parameterInfo, CommandInfo parent, string nameSpace)
        {
            Throw.IfNullOrWhiteSpace(project);
            Throw.IfNull(() => parameterInfo);
            Throw.IfNull(() => parent);
            Throw.IfNullOrWhiteSpace(nameSpace);

            var commandHandler = _commandHandlerBuilder.Build(parameterInfo);
            var newTemplate = Template.Replace("$command-name$", parameterInfo.NormalizedName)
                .Replace("$command-argument-name$", parameterInfo.NormalizedName.FirstCharToLower())
                .Replace("$namespace$", nameSpace)
                .Replace("$command-description$", parameterInfo.Description)
                .Replace("$command-handler$", commandHandler)
                .Replace("$parent-command-name$", parent.NormalizedName)
                .Replace("$project-name$", project);

            return newTemplate;
        }
    }
}
