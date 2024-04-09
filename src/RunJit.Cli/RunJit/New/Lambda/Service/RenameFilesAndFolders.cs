using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.New.Lambda
{
    internal static class AddRenameFilesAndFoldersExtension
    {
        internal static void AddRenameFilesAndFolders(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<RenameFilesAndFolders>();
        }
    }

    internal class RenameFilesAndFolders
    {
        public void Rename(DirectoryInfo directoryInfo, string originalName, string newName)
        {
            var folders = directoryInfo.EnumerateDirectories("*.*", SearchOption.AllDirectories);
            foreach (var folder in folders)
            {
                if (folder.Name.Contains(originalName))
                {
                    Directory.Move(folder.FullName, folder.FullName.Replace(originalName, newName));
                }
            }

            var allFiles = directoryInfo.EnumerateFiles("*.*", SearchOption.AllDirectories);
            foreach (var fileInfo in allFiles)
            {
                var content = File.ReadAllText(fileInfo.FullName);
                if (content.Contains(originalName))
                {
                    var newContent = content.Replace(originalName, newName);
                    File.WriteAllText(fileInfo.FullName, newContent);
                }

                if (fileInfo.Name.Contains(originalName))
                {
                    File.Move(fileInfo.FullName, fileInfo.FullName.Replace(originalName, newName));
                }
            }
        }
    }
}
