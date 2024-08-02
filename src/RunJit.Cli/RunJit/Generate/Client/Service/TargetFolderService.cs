using Argument.Check;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.ErrorHandling;

namespace RunJit.Cli.RunJit.Generate.Client
{
    internal static class AddTargetFolderServiceExtension
    {
        internal static void AddTargetFolderService(this IServiceCollection services)
        {
            services.AddSolutionFileModifier();

            services.AddSingletonIfNotExists<ITargetFolderService, TargetFolderService>();
        }
    }

    internal interface ITargetFolderService
    {
        DirectoryInfo CreateTargetDirectory(Client client);

        DirectoryInfo GetToolFolder(Client client,
                                    DirectoryInfo targetDirectoryInfo);

        FileInfo GetSolutionFile(Client client,
                                 DirectoryInfo targetDirectoryInfo);
    }

    internal class TargetFolderService : ITargetFolderService
    {
        public DirectoryInfo CreateTargetDirectory(Client client)
        {
            Throw.IfNull(client.SolutionFileInfo.Directory);

            var foldersToDelete = client.SolutionFileInfo.Directory.EnumerateDirectories(client.ProjectName).ToList();

            foreach (var directoryInfo in foldersToDelete)
            {
                directoryInfo.Delete(true);
            }

            return client.SolutionFileInfo.Directory;
        }

        public DirectoryInfo GetToolFolder(Client client,
                                           DirectoryInfo targetDirectoryInfo)
        {
            if (targetDirectoryInfo.NotExists())
            {
                throw new RunJitException($"The directory: '{targetDirectoryInfo.FullName}' does not exists.");
            }

            var toolFolder = new DirectoryInfo(Path.Combine(targetDirectoryInfo.FullName, "src", client.ProjectName,
                                                            client.DotNetToolName.NormalizedName));

            if (toolFolder.NotExists())
            {
                throw new RunJitException($"The tool folder: '{toolFolder.FullName}' does not exists.");
            }

            return toolFolder;
        }

        public FileInfo GetSolutionFile(Client client,
                                        DirectoryInfo targetDirectoryInfo)
        {
            if (targetDirectoryInfo.NotExists())
            {
                throw new RunJitException($"The directory: '{targetDirectoryInfo.FullName}' does not exists.");
            }

            return client.SolutionFileInfo;
        }
    }
}
