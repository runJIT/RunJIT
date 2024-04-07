using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Generate.Client
{
    internal static class AddModelFolderBuilderExtension
    {
        internal static void AddModelFolderBuilder(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<ModelFolderBuilder>();
        }
    }

    internal class ModelFolderBuilder
    {
        internal DirectoryInfo Build(DirectoryInfo versionFolder)
        {
            var modelsFolder = new DirectoryInfo(Path.Combine(versionFolder.FullName, "Models"));
            if (modelsFolder.Exists.IsFalse())
            {
                modelsFolder.Create();
            }

            return modelsFolder;
        }
    }
}
