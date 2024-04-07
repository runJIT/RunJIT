using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;

namespace RunJit.Cli.RunJit.Generate.Client
{
    internal static class AddTemplateServiceExtension
    {
        internal static void AddTemplateService(this IServiceCollection services)
        {
            services.AddRenameFilesAndFolders();

            services.AddSingletonIfNotExists<ITemplateService, TemplateService>();
        }
    }

    internal interface ITemplateService
    {
        void RenameAllIn(DirectoryInfo targetDirectory, Client client);
    }

    internal class TemplateService(IRenameFilesAndFolders renameFilesAndFolders) : ITemplateService
    {
        public void RenameAllIn(DirectoryInfo targetDirectory, Client client)
        {
            renameFilesAndFolders.Rename2(targetDirectory, "rps.template", client.ProjectName);
        }
    }
}
