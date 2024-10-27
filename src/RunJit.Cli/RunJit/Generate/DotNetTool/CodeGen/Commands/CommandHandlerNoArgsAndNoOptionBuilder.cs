using Argument.Check;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using Solution.Parser.CSharp;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    public static class AddCommandHandlerNoArgsAndNoOptionBuilderExtension
    {
        public static void AddCommandHandlerNoArgsAndNoOptionBuilder(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<ICommandHandlerStringBuilder, CommandHandlerNoArgsAndNoOptionBuilder>();
        }
    }

    internal sealed class CommandHandlerNoArgsAndNoOptionBuilder : ICommandHandlerStringBuilder
    {
        private const string Template = "CommandHandler.Create(() => $command-argument-name$Handler.HandleAsync(new $command-name$Parameters($argument-names$)))";

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
            return parameterInfo.Argument.IsNull() && parameterInfo.Options.IsNullOrEmpty();
        }

        private IEnumerable<CtorArgument> BuildCtorArguments(CommandInfo parameterInfo)
        {
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
