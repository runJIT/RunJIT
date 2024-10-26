using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    public static class AddConstructorArgumentBuilderExtension
    {
        public static void AddConstructorArgumentBuilder(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<PrimaryConstructorArgumentBuilder>();
        }
    }

    internal sealed class PrimaryConstructorArgumentBuilder
    {
        public IEnumerable<CtorArgument> Build(CommandInfo parameterInfo)
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
                    yield return new CtorArgument(optionInfo.Argument.OptimizedType, ((string)optionInfo.NormalizedName).FirstCharToUpper());
                }
                else
                {
                    yield return new CtorArgument("bool", ((string)optionInfo.NormalizedName).FirstCharToUpper());
                }
            }
        }
    }
}
