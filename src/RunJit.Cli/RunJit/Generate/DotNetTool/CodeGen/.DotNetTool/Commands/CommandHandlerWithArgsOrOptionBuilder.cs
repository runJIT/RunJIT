using Argument.Check;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using Solution.Parser.CSharp;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
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

        public string Build(Models.CommandInfo parameterInfo)
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

        public bool IsThisBuilderFor(Models.CommandInfo parameterInfo)
        {
            Throw.IfNull(() => parameterInfo);

            return parameterInfo.Argument.IsNotNull() || parameterInfo.Options.Any();
        }

        private IEnumerable<Models.CtorArgument> BuildCtorArguments(Models.CommandInfo parameterInfo)
        {
            Throw.IfNull(() => parameterInfo);

            var argumentInfo = parameterInfo.Argument;

            if (argumentInfo.IsNotNull())
            {
                yield return new Models.CtorArgument(argumentInfo.OptimizedType, ((string)argumentInfo.NormalizedName).FirstCharToLower());
            }

            foreach (var optionInfo in parameterInfo.Options)
            {
                if (optionInfo.Argument.IsNotNull())
                {
                    yield return new Models.CtorArgument(optionInfo.Argument.OptimizedType, ((string)optionInfo.NormalizedName).FirstCharToLower());
                }
                else
                {
                    yield return new Models.CtorArgument("bool", ((string)optionInfo.NormalizedName).FirstCharToLower());
                }
            }
        }
    }
}
