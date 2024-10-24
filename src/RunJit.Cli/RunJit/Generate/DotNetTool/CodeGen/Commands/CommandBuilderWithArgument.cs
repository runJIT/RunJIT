using Argument.Check;
using RunJit.Cli.RunJit.Generate.DotNetTool.CodeGen.Models;

namespace RunJit.Cli.RunJit.Generate.DotNetTool.CodeGen.Commands
{
    internal sealed class CommandBuilderWithArgument : ICommandBuilderWithArgument
    {
        private const string Template =
@"using System.CommandLine;
using System.CommandLine.Invocation;    
using $namespace$.Arguments;  
using $namespace$.Service;

namespace $namespace$
{                    
    internal sealed class $command-name$CommandBuilder : I$parent-command-name$SubCommandBuilder
    {
        private readonly I$command-name$Service _$command-service-argument-name$Service;        
        private readonly I$command-name$ArgumentBuilder _argumentBuilder;

        public $command-name$CommandBuilder(I$command-name$Service $command-service-argument-name$Service, I$command-name$ArgumentBuilder argumentBuilder)
        {                    
            _$command-service-argument-name$Service = $command-service-argument-name$Service;            
            _argumentBuilder = argumentBuilder;
        }

        public Command Build()
        {
            var command = new Command(""$command-argument-name$"", ""$command-description$"");            
            command.AddArgument(_argumentBuilder.Build());
            command.Handler = $command-handler$;
            return command;
        }
    }
}";

        private readonly ICommandHandlerBuilder _commandHandlerBuilder;

        public CommandBuilderWithArgument(ICommandHandlerBuilder commandHandlerBuilder)
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
                .Replace("$parent-command-name$", parent.NormalizedName)
                .Replace("$command-argument-name$", parameterInfo.Name)
                .Replace("$command-description$", parameterInfo.Description)
                .Replace("$command-service-argument-name$", parameterInfo.Name)
                .Replace("$command-handler$", commandHandler)
                .Replace("$namespace$", nameSpace)
                .Replace("$project-name$", project);

            return newTemplate;
        }
    }
}
