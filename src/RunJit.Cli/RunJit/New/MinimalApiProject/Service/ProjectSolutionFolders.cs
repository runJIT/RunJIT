using System.Xml.Linq;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;

namespace RunJit.Cli.New.MinimalApiProject
{
    internal static class AddProjectSolutionFoldersExtension
    {
        internal static void AddProjectSolutionFolders(this IServiceCollection services)
        {
            services.AddRetryHelper();

            services.AddSingletonIfNotExists<IMinimalApiProjectSpecificCodeGen, ProjectSolutionFolders>();
        }
    }

    internal sealed class ProjectSolutionFolders() : IMinimalApiProjectSpecificCodeGen
    {
        public Task GenerateAsync(FileInfo projectFileInfo,
                                        FileInfo solutionFile,
                                        XDocument projectDocument,
                                        MinimalApiProjectInfos minimalApiProjectInfos)
        {
            // Solution folders setup
            // Api
            // -> Contains the Web-Api
            // SolutionItems
            // -> .editorconfig
            // -> Directory.Build.props
            // Nuget
            // -> NuGet.Config
            // Docu
            // -> contains all readme
            // CI/CD
            // -> contains all ci/cd files
            // Resharper
            // -> *.DotSettings
            // Git
            // -> commitlint.config.js
            // -> .gitignore
            // -> repolinter.json

            // !! WIP !!

            // GitHub we add anything
            //var gitHub = solutionFile.Directory!.EnumerateDirectories().FirstOrDefault(item => item.Name == ".github");
            //if (gitHub.IsNotNull())
            //{
            //    var allGitHubFiles = gitHub.EnumerateFiles("*", SearchOption.AllDirectories);
            //    foreach (var fileInfo in allGitHubFiles)
            //    {
            //        // Relative solution folder path
            //        var relativePath = Path.GetRelativePath(gitHub.FullName, fileInfo.Directory!.FullName);
            //        var withGitHubFolder = Path.Combine("GitHub", relativePath);
            //        var relativeFilePath  = new FileInfo(Path.GetRelativePath(solutionFile.Directory.Name, fileInfo.FullName));
            //        await dotNet.AddFileIntoSoltionAsync(solutionFile, relativeFilePath, withGitHubFolder);
            //    }
            //}

            //var filesOnSolutionRoot = solutionFile.Directory!.EnumerateFiles();
            //foreach (var fileInfo in filesOnSolutionRoot)
            //{
            //    // Any markdown we will add to docs
            //    if (fileInfo.Extension == ".md")
            //    {
            //        await dotNet.AddFileIntoSoltionAsync(solutionFile, fileInfo, "Docs");
            //    }

            //    if (fileInfo.Extension == ".DotSettings")
            //    {
            //        await dotNet.AddFileIntoSoltionAsync(solutionFile, fileInfo, "Resharper");
            //    }

            //    if (fileInfo.Extension == ".gitignore" ||
            //        fileInfo.Name == "commitlint.config.js")
            //    {
            //        await dotNet.AddFileIntoSoltionAsync(solutionFile, fileInfo, "Git");
            //    }

            //    if (fileInfo.Extension == ".gitignore" ||
            //        fileInfo.Name == "commitlint.config.js" ||
            //        fileInfo.Name == "repolinter.json")
            //    {
            //        await dotNet.AddFileIntoSoltionAsync(solutionFile, fileInfo, "Git");
            //    }
            //}

            return Task.CompletedTask;
        }
    }
}
