using Argument.Check;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Generate.DotNetTool.Models;
using Solution.Parser.CSharp;

namespace RunJit.Cli.Generate.DotNetTool
{
    internal static class AddCommandHandlerWithArgsOrOptionBuilderExtension
    {
        internal static void AddCommandHandlerWithArgsOrOptionBuilder(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<ICommandHandlerStringBuilder, CommandHandlerWithArgsOrOptionBuilder>();
        }
    }

    internal sealed class CommandHandlerWithArgsOrOptionBuilder : ICommandHandlerStringBuilder
    {
        private const string Template = "CommandHandler.Create<$types$>(($argument-names$) => $command-argument-name$Handler.HandleAsync(new $command-name$Parameters($argument-names$)))";

        public string Build(CommandInfo parameterInfo)
        {
            var arguments = BuildCtorArguments(parameterInfo).ToList();
            var types = arguments.Select(arg => arg.Type).Flatten(", ");
            var argNames = arguments.Select(arg => arg.Name).Flatten(", ");

            var newTemplate = Template.Replace("$types$", types)
                                      .Replace("$command-name$", parameterInfo.NormalizedName)
                                      .Replace("$command-argument-name$", parameterInfo.NormalizedName.FirstCharToLower())
                                      .Replace("$argument-names$", argNames);

            return newTemplate.FormatSyntaxTree();
        }

        public bool IsThisBuilderFor(CommandInfo parameterInfo)
        {
            Throw.IfNull(() => parameterInfo);

            return parameterInfo.Argument.IsNotNull() || parameterInfo.Options.Any();
        }

        private IEnumerable<CtorArgument> BuildCtorArguments(CommandInfo parameterInfo)
        {
            Throw.IfNull(() => parameterInfo);

            var argumentInfo = parameterInfo.Argument;

            if (argumentInfo.IsNotNull())
            {
                yield return new CtorArgument(argumentInfo.OptimizedType, ((string)argumentInfo.NormalizedName).FirstCharToLower());
            }

            foreach (var optionInfo in parameterInfo.Options)
            {
                if (optionInfo.Argument.IsNotNull())
                {
                    yield return new CtorArgument(optionInfo.Argument.OptimizedType, ((string)optionInfo.NormalizedName).FirstCharToLower());
                }
                else
                {
                    yield return new CtorArgument("bool", ((string)optionInfo.NormalizedName).FirstCharToLower());
                }
            }
        }
    }
}
