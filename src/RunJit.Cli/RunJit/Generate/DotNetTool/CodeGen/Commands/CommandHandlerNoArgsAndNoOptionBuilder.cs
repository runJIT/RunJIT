using Argument.Check;
using Extensions.Pack;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal sealed class CommandHandlerNoArgsAndNoOptionBuilder : ICommandHandlerStringBuilder
    {
        private const string Template = "CommandHandler.Create(() => _$command-argument-name$Service.HandleAsync(new $command-name$Parameters($argument-names$)))";

        public string Build(CommandInfo parameterInfo)
        {
            Throw.IfNull(() => parameterInfo);

            var arguments = BuildCtorArguments(parameterInfo).ToList();
            var types = arguments.Select(arg => arg.Type).Flatten(", ");
            var argNames = arguments.Select(arg => arg.Name).Flatten(", ");

            var newTemplate = Template.Replace("$types$", types)
                                      .Replace("$command-name$", parameterInfo.NormalizedName)
                                      .Replace("$command-argument-name$", parameterInfo.NormalizedName.FirstCharToLower())
                                      .Replace("$argument-names$", argNames);
            return newTemplate;
        }

        public bool IsThisBuilderFor(CommandInfo parameterInfo)
        {
            Throw.IfNull(() => parameterInfo);

            return ObjectExtensions.IsNull((object?)parameterInfo.Argument) && parameterInfo.Options.IsNullOrEmpty();
        }

        private IEnumerable<CtorArgument> BuildCtorArguments(CommandInfo parameterInfo)
        {
            var argumentInfo = parameterInfo.Argument;
            if (ObjectExtensions.IsNotNull((object?)argumentInfo))
            {
                yield return new CtorArgument(argumentInfo.OptimizedType, global::Extensions.Pack.StringExtensions.FirstCharToLower((string)argumentInfo.NormalizedName));
            }

            foreach (var optionInfo in parameterInfo.Options)
            {
                if (ObjectExtensions.IsNotNull((object?)optionInfo.Argument))
                {
                    yield return new CtorArgument(optionInfo.Argument.OptimizedType, global::Extensions.Pack.StringExtensions.FirstCharToLower((string)optionInfo.NormalizedName));
                }
                else
                {
                    yield return new CtorArgument("bool", global::Extensions.Pack.StringExtensions.FirstCharToLower((string)optionInfo.NormalizedName));
                }
            }
        }
    }
}
