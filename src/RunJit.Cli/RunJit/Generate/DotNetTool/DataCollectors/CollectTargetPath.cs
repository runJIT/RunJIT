using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal static class AddCollectTargetPathExtension
    {
        internal static void AddCollectTargetPath(this IServiceCollection services)
        {
            services.AddTargetPathValidator();
            services.AddCollectTillInputCorrect();

            services.AddSingletonIfNotExists<CollectTargetPath>();
        }
    }

    internal class CollectTargetPath(TargetPathValidator inputValidator,
                                     ICollectTillInputCorrect collectTillInputCorrect)
    {
        private const string Title = @"Please enter the target path where the client should be generated. Sample: D:\DotNetTools\";

        public DirectoryInfo Collect()
        {
            var directoryPath = collectTillInputCorrect.CollectTillInputIsValid(Title, inputValidator);

            return new DirectoryInfo(directoryPath);
        }
    }
}
