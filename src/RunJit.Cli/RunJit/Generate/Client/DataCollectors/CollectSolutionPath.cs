using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Generate.Client
{
    internal static class AddCollectSolutionPathExtension
    {
        internal static void AddCollectSolutionPath(this IServiceCollection services)
        {
            services.AddSolutionFileValidator();
            services.AddCollectTillInputCorrect();

            services.AddSingletonIfNotExists<CollectSolutionPath>();
        }
    }

    internal class CollectSolutionPath(SolutionFileValidator inputValidator,
                                       ICollectTillInputCorrect collectTillInputCorrect)
    {
        private const string Title = @"Please enter the full path to your backend solution. Sample: D:\Projetcs\ClientGen\ClientGen.sln";

        public FileInfo Collect()
        {
            var solutionFile = collectTillInputCorrect.CollectTillInputIsValid(Title, inputValidator);

            return new FileInfo(solutionFile);
        }
    }
}
