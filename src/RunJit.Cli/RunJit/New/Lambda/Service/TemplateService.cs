using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.New.Lambda
{
    public static class AddTemplateServiceExtension
    {
        public static void AddTemplateService(this IServiceCollection services)
        {
            services.AddRenameFilesAndFolders();
            services.AddSingletonIfNotExists<TemplateService>();
        }
    }

    internal class TemplateService(RenameFilesAndFolders renameFilesAndFolders)
    {
        public void RenameAllIn(DirectoryInfo targetDirectory,
                                LambdaInfos lambdaInfos)
        {
            // Solution and projects
            renameFilesAndFolders.Rename(targetDirectory, "rps.template", lambdaInfos.ProjectName);

            renameFilesAndFolders.Rename(targetDirectory, "$lambda-name$", lambdaInfos.Parameters.LambdaName);

            // DotNetTool name
            // class etc. and rest
            renameFilesAndFolders.Rename(targetDirectory, "Rps", lambdaInfos.Parameters.FunctionName);

            renameFilesAndFolders.Rename(targetDirectory, "$module$", lambdaInfos.Parameters.ModuleName);
        }
    }
}
