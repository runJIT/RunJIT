using Argument.Check;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    public static class AddCommandBuilderWithArgumentExtension
    {
        public static void AddCommandBuilderWithArgument(this IServiceCollection services)
        {
            services.AddCommandHandlerBuilder();

            services.AddSingletonIfNotExists<CommandBuilderWithArgument>();
        }
    }

    internal sealed class CommandBuilderWithArgument
    {
        private const string Template =
            @"using System.CommandLine;
using System.CommandLine.Invocation;    
using $namespace$.Arguments;  
using $namespace$.Service;

namespace $namespace$
{                    
    internal sealed class $command-name$CommandBuilder(I$command-name$Service $command-service-argument-name$Service, 
                                                       I$command-name$ArgumentBuilder argumentBuilder)$interface$
    {       
        public Command Build()
        {
            var command = new Command(""$command-argument-name$"", ""$command-description$"");            
            command.AddArgument(_argumentBuilder.Build());
            command.Handler = $command-handler$;
            return command;
        }
    }
}";

        private readonly CommandHandlerBuilder _commandHandlerBuilder;

        public CommandBuilderWithArgument(CommandHandlerBuilder commandHandlerBuilder)
        {
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
                                      .Replace("$command-argument-name$", commandInfo.Name)
                                      .Replace("$command-description$", commandInfo.Description)
                                      .Replace("$command-service-argument-name$", commandInfo.Name)
                                      .Replace("$command-handler$", commandHandler)
                                      .Replace("$namespace$", nameSpace)
                                      .Replace("$project-name$", project)
                                      .Replace("$interface$", interfaceImplementation);

            return newTemplate;
        }
    }
}
