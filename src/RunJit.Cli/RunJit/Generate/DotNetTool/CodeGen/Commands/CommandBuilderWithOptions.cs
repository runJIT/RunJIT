﻿using Argument.Check;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using Solution.Parser.CSharp;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    public static class AddCommandBuilderWithOptionsExtension
    {
        public static void AddCommandBuilderWithOptions(this IServiceCollection services)
        {
            services.AddCommandHandlerBuilder();

            services.AddSingletonIfNotExists<CommandBuilderWithOptions>();
        }
    }

    internal sealed class CommandBuilderWithOptions
    {
        private const string Template =
            @"
using System.CommandLine;
using System.CommandLine.Invocation;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace $namespace$
{                    
    internal static class Add$command-name$CommandBuilderExtension
    {
        internal static void Add$command-name$CommandBuilder(this IServiceCollection services, IConfiguration configuration)
        {
            services.Add$command-name$Handler(configuration);
            services.Add$command-name$OptionsBuilder();

            services.AddSingletonIfNotExists<$commandRegistration$>();
        }
    }

    internal sealed class $command-name$CommandBuilder($command-name$Handler $command-handler-argument-name$Handler, 
                                                       $command-name$OptionsBuilder optionsBuilder)$interface$
    {
        public Command Build()
        {
            var command = new Command(""$command-argument-name$"", ""$command-description$"");
            optionsBuilder.Build().ToList().ForEach(option => command.AddOption(option));            
            command.Handler = $command-handler$;
            return command;
        }
    }
}";

        private readonly CommandHandlerBuilder _commandHandlerBuilder;

        public CommandBuilderWithOptions(CommandHandlerBuilder commandHandlerBuilder)
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
            var commandRegistration = parentCommandInfo.IsNull() || commandInfo == parentCommandInfo ? $"{commandInfo.NormalizedName}CommandBuilder" : $"I{parentCommandInfo.NormalizedName}SubCommandBuilder, {commandInfo.NormalizedName}CommandBuilder";

            var newTemplate = Template.Replace("$command-name$", commandInfo.NormalizedName)
                                      .Replace("$command-description$", commandInfo.Description)
                                      .Replace("$command-argument-name$", commandInfo.NormalizedName.ToLowerInvariant())
                                      .Replace("$command-handler-argument-name$", commandInfo.Name.FirstCharToLower())
                                      .Replace("$command-handler$", commandHandler)
                                      .Replace("$namespace$", nameSpace)
                                      .Replace("$project-name$", project)
                                      .Replace("$interface$", interfaceImplementation)
                                      .Replace("$commandRegistration$", commandRegistration);;

            return newTemplate.FormatSyntaxTree();
        }
    }
}
