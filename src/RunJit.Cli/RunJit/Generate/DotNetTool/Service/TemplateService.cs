using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
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
        void RenameAllIn(DirectoryInfo targetDirectory,
                         DotNetTool client);
    }

    internal class TemplateService(IRenameFilesAndFolders renameFilesAndFolders) : ITemplateService
    {
        public void RenameAllIn(DirectoryInfo targetDirectory,
                                DotNetTool client)
        {
            renameFilesAndFolders.Rename2(targetDirectory, "rps.template", client.ProjectName);
        }
    }
}
