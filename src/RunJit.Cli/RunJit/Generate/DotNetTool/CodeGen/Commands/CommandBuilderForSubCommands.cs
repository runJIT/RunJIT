using Argument.Check;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using Pipelines.Sockets.Unofficial.Arenas;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    public static class AddCommandBuilderForSubCommandsExtension
    {
        public static void AddCommandBuilderForSubCommands(this IServiceCollection services)
        {
            services.AddCommandHandlerBuilder();

            services.AddSingletonIfNotExists<CommandBuilderForSubCommands>();
        }
    }

    internal sealed class CommandBuilderForSubCommands
    {
        private const string Template = @"
using System.CommandLine;
using System.CommandLine.Invocation;  

namespace $namespace$
{    
    internal sealed class $command-name$CommandBuilder(IEnumerable<I$command-name$SubCommandBuilder> $command-argument-name$SubCommandBuilders)$interface$
    {     
        public Command Build()
        {
            var $command-argument-name$Command = new Command(""$command-argument-name$"", ""$command-description$"");
            $command-argument-name$SubCommandBuilders.ToList().ForEach(builder => $command-argument-name$Command.AddCommand(builder.Build()));            
            return $command-argument-name$Command;
        }
    }
}";

        private readonly CommandHandlerBuilder _commandHandlerBuilder;

        public CommandBuilderForSubCommands(CommandHandlerBuilder commandHandlerBuilder)
        {
            Throw.IfNull(() => commandHandlerBuilder);

            _commandHandlerBuilder = commandHandlerBuilder;
        }

        public string Build(string project,
                            CommandInfo commandInfo,
                            CommandInfo? parentCommandInfo,
                            string nameSpace)
        {
            Throw.IfNullOrWhiteSpace(project);
            Throw.IfNullOrWhiteSpace(nameSpace);

            var commandHandler = _commandHandlerBuilder.Build(commandInfo);

            var interfaceImplementation = parentCommandInfo.IsNull() || commandInfo == parentCommandInfo ? string.Empty : $" : I{parentCommandInfo.NormalizedName}SubCommandBuilder";

            var newTemplate = Template.Replace("$command-name$", commandInfo.NormalizedName)
                                      .Replace("$command-argument-name$", commandInfo.NormalizedName.FirstCharToLower())
                                      .Replace("$namespace$", nameSpace)
                                      .Replace("$command-description$", commandInfo.Description)
                                      .Replace("$command-handler$", commandHandler)
                                      .Replace("$project-name$", project)
                                      .Replace("$interface$", interfaceImplementation);

            return newTemplate;
        }
    }
}
