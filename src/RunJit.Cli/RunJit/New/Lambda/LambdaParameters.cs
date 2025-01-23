using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.New.Lambda
{
    internal static class AddLambdaParametersExtension
    {
        internal static void AddLambdaParameters(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<LambdaParameters>();
        }
    }

    internal sealed class LambdaParameters(FileInfo solution,
                                           string moduleName,
                                           string functionName,
                                           string lambdaName)
    {
        public FileInfo Solution { get; } = solution;

        public string ModuleName { get; } = moduleName;

        public string FunctionName { get; } = functionName;

        public string LambdaName { get; } = lambdaName;
    }
}
