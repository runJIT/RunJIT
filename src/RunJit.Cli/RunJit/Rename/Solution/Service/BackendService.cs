using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.ErrorHandling;
using RunJit.Cli.Services;

namespace RunJit.Cli.RunJit.Rename.Solution
{
    internal static class AddBackendServiceExtension
    {
        internal static void AddBackendService(this IServiceCollection services)
        {
            services.AddConsoleService();
            services.AddBackendParameters();
            services.AddFindSolutionFile();

            services.AddSingletonIfNotExists<IBackendService, BackendService>();
        }
    }

    internal interface IBackendService
    {
        Task HandleAsync(BackendParameters parameters);
    }

    internal sealed class BackendService(ConsoleService consoleService,
                                         IRenameFilesAndFolders renameFilesAndFolders,
                                         FindSolutionFile findSolutionFile) : IBackendService
    {
        public Task HandleAsync(BackendParameters parameters)
        {
            // 1. Check if solution file is the file or directory
            //    if it is null or whitespace we check current directory
            var solutionFile = findSolutionFile.Find(parameters.FileOrFolder);
            var currentDirectory = solutionFile.Directory!;

            // 2. Kill all debug and obj folders first
            var binFolders = currentDirectory.EnumerateDirectories("bin", SearchOption.AllDirectories).ToList();

            binFolders.ForEach(folder =>
                               {
                                   try
                                   {
                                       folder.Delete(true);
                                   }
                                   catch (Exception e)
                                   {
                                       Console.WriteLine(e);
                                   }
                               });

            var objFolders = currentDirectory.EnumerateDirectories("obj", SearchOption.AllDirectories).ToList();

            objFolders.ForEach(folder =>
                               {
                                   try
                                   {
                                       folder.Delete(true);
                                   }
                                   catch (Exception e)
                                   {
                                       Console.WriteLine(e);
                                   }
                               });

            var newDirectoryInfo = renameFilesAndFolders.Rename(solutionFile.Directory!, parameters.OldName, parameters.NewName);

            var newSolutionFile = newDirectoryInfo.EnumerateFiles("*.sln").FirstOrDefault();

            if (newSolutionFile.IsNull())
            {
                throw new RunJitException($"The new expected solution file with the name {parameters.NewName} was not found in the current directory: {currentDirectory.FullName}");
            }

            consoleService.WriteSuccess($"Backend successfully renamed from: {parameters.OldName} into {parameters.NewName}");
            consoleService.WriteSuccess(newSolutionFile.FullName);

            return Task.CompletedTask;
        }
    }
}
