using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal static class AddConstructorArgumentBuilderExtension
    {
        internal static void AddConstructorArgumentBuilder(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<PrimaryConstructorArgumentBuilder>();
        }
    }

    internal sealed class PrimaryConstructorArgumentBuilder
    {
        public IEnumerable<Models.CtorArgument> Build(Models.CommandInfo parameterInfo)
        {
            var argumentInfo = parameterInfo.Argument;

            if (argumentInfo.IsNotNull())
            {
                yield return new Models.CtorArgument(argumentInfo.OptimizedType, ((string)argumentInfo.NormalizedName).FirstCharToLower());
            }

            foreach (var optionInfo in parameterInfo.Options)
            {
                if (optionInfo.Argument.IsNotNull())
                {
                    yield return new Models.CtorArgument(optionInfo.Argument.OptimizedType, ((string)optionInfo.NormalizedName).FirstCharToUpper());
                }
                else
                {
                    yield return new Models.CtorArgument("bool", ((string)optionInfo.NormalizedName).FirstCharToUpper());
                }
            }
        }
    }
}
