using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Generate.Client
{
    internal static class AddDomainFolderBuilderExtension
    {
        internal static void AddDomainFolderBuilder(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<DomainFolderBuilder>();
        }
    }

    internal class DomainFolderBuilder
    {
        internal DirectoryInfo Build(DirectoryInfo apiFolder, GeneratedFacade generatedFacade)
        {
            var domainFolder = new DirectoryInfo(Path.Combine(apiFolder.FullName, generatedFacade.Domain));
            if (domainFolder.Exists.IsFalse())
            {
                domainFolder.Create();
            }

            return domainFolder;
        }
    }
}
