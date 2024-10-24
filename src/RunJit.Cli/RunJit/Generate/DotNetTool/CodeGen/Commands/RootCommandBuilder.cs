using Argument.Check;
using Extensions.Pack;
using RunJit.Cli.RunJit.Generate.DotNetTool.CodeGen.Models;

namespace RunJit.Cli.RunJit.Generate.DotNetTool.CodeGen.Commands
{
    internal sealed class RootCommandBuilder : IRootCommandBuilder
    {
        private const string Template =
@"using System.CommandLine;    

namespace $namespace$
{    
    internal sealed class $command-name$CommandBuilder : I$command-name$CommandBuilder
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

        private readonly ICommandHandlerStringBuilder _commandHandlerStringBuilder;

        public RootCommandBuilder(ICommandHandlerStringBuilder commandHandlerStringBuilder)
        {
            Throw.IfNull(() => commandHandlerStringBuilder);

            _commandHandlerStringBuilder = commandHandlerStringBuilder;
        }

        public string Build(string project, CommandInfo parameterInfo, string nameSpace)
        {
            Throw.IfNullOrWhiteSpace(project);
            Throw.IfNull(() => parameterInfo);
            Throw.IfNullOrWhiteSpace(nameSpace);

            var commandHandler = _commandHandlerStringBuilder.Build(parameterInfo);

            var newTemplate = Template.Replace("$command-name$", parameterInfo.NormalizedName)
                .Replace("$command-argument-name$", parameterInfo.NormalizedName.FirstCharToLower())
                .Replace("$namespace$", nameSpace)
                .Replace("$command-description$", parameterInfo.Description)
                .Replace("$command-handler$", commandHandler)
                .Replace("$project-name$", project);

            return newTemplate;
        }
    }
}
