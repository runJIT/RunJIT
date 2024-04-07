using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using Solution.Parser.Project;

namespace RunJit.Cli.RunJit.Generate.Client
{
    internal static class AddApiFolderBuilderExtension
    {
        internal static void AddApiFolderBuilder(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<ApiFolderBuilder>();
        }
    }

    internal sealed class ApiFolderBuilder
    {
        internal DirectoryInfo Build(ProjectFile clientProject)
        {
            var apiFolder = new DirectoryInfo(Path.Combine(clientProject.ProjectFileInfo.Value.Directory!.FullName, ClientGenConstants.Api));
            if (apiFolder.Exists)
            {
                apiFolder.Delete(true);
            }

            apiFolder.Create();

            return apiFolder;
        }
    }


    internal static class ClientGenConstants
    {
        internal static string Api { get; } = "Api";
    }
}
