﻿using System.Xml.Linq;
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
            services.AddSolutionFileService();

            services.AddSingletonIfNotExists<IMinimalApiProjectSpecificCodeGen, ProjectSolutionFolders>();
        }
    }

    internal sealed class ProjectSolutionFolders(SolutionFileService solutionFileService) : IMinimalApiProjectSpecificCodeGen
    {
        public async Task GenerateAsync(FileInfo projectFileInfo,
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



            var solutionFileLines = await File.ReadAllLinesAsync(solutionFile.FullName).ConfigureAwait(false);
            var solutionFilesAsLines = solutionFileLines.IsNull() ? new List<string>() : solutionFileLines.ToList();

            // GitHub we add anything
            // Recursive directory / files structure into solution files
            // is insane sick > comes later
            //var gitHub = solutionFile.Directory!.EnumerateDirectories().FirstOrDefault(item => item.Name == ".github");
            //if (gitHub.IsNotNull())
            //{
            //    // Create root folder, Root -> Bottom
            //    solutionFileService.AddOrUpdateSolutionFolder(solutionFilesAsLines, solutionFile, "GitHub");

            //    foreach (var directory in gitHub.EnumerateDirectories())
            //    {
            //        // Create root folder, Root -> Bottom
            //        solutionFileService.AddOrUpdateSolutionFolder(solutionFilesAsLines, solutionFile, directory.Name);
            //    }

            //    var allGitHubFiles = gitHub.EnumerateFiles("*", SearchOption.AllDirectories);
            //    foreach (var fileInfo in allGitHubFiles)
            //    {
            //        // Relative solution folder path
            //        var relativePath = Path.GetRelativePath(gitHub.FullName, fileInfo.Directory!.FullName);
            //        var withGitHubFolder = Path.Combine("GitHub", relativePath);
            //        solutionFileService.AddOrUpdateSolutionFolder(solutionFilesAsLines, solutionFile, withGitHubFolder, fileInfo);
            //    }
            //}

            var filesOnSolutionRoot = solutionFile.Directory!.EnumerateFiles();

            foreach (var fileInfo in filesOnSolutionRoot)
            {
                // Any markdown we will add to docs
                if (fileInfo.Extension == ".md")
                {
                    solutionFilesAsLines = solutionFileService.AddOrUpdateSolutionFolder(solutionFilesAsLines, solutionFile, "Docs", fileInfo);
                }

                if (fileInfo.Extension == ".DotSettings")
                {
                    solutionFilesAsLines = solutionFileService.AddOrUpdateSolutionFolder(solutionFilesAsLines, solutionFile, "Resharper", fileInfo);
                }

                if (fileInfo.Extension == ".gitignore" ||
                    fileInfo.Name == "commitlint.config.js" ||
                    fileInfo.Name == "repolinter.json")
                {
                    solutionFilesAsLines = solutionFileService.AddOrUpdateSolutionFolder(solutionFilesAsLines, solutionFile, "Git", fileInfo);
                }

                if (fileInfo.Extension == ".editorconfig" ||
                    fileInfo.Extension == ".runsettings")
                {
                    solutionFilesAsLines = solutionFileService.AddOrUpdateSolutionFolder(solutionFilesAsLines, solutionFile, "SolutionItems", fileInfo);
                }
            }

            await File.WriteAllLinesAsync(solutionFile.FullName, solutionFilesAsLines).ConfigureAwait(false);
        }
    }
}
