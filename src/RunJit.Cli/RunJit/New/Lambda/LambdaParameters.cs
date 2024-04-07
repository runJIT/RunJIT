using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.New.Lambda
{
    public static class AddLambdaParametersExtension
    {
        public static void AddLambdaParameters(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<LambdaParameters>();
        }
    }

    internal record LambdaParameters(FileInfo Solution,
                                     string ModuleName,
                                     string FunctionName,
                                     string LambdaName,
                                     string GitRepos,
                                     string Branch,
                                     string WorkingDirectory);
}
