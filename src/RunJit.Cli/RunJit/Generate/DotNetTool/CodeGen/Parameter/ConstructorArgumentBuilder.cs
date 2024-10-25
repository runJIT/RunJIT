using Extensions.Pack;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal sealed class ConstructorArgumentBuilder : IConstructorArgumentBuilder
    {
        public IEnumerable<CtorArgument> Build(CommandInfo parameterInfo)
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
