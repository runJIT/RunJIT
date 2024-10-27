using System.Text.RegularExpressions;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;

namespace RunJit.Cli.RunJit.Update.Net
{
    public static class AddDotNetServiceExtension
    {
        public static void AddDotNetService(this IServiceCollection services)
        {
            services.AddConsoleService();
            services.AddDotNetParameters();
            services.AddFindSolutionFile();

            services.AddSingletonIfNotExists<IDotNetService, DotNetService>();
        }
    }

    internal interface IDotNetService
    {
        Task HandleAsync(DotNetParameters parameters);
    }

    internal sealed class DotNetService(ConsoleService consoleService,
                                 FindSolutionFile findSolutionFile) : IDotNetService
    {
        private readonly Regex _versionReplaceRegex = new(@"(\d+\.\d-+)", RegexOptions.Compiled);

        readonly Regex _netVersionReplaceRegex = new(@"net\d+\.\d+", RegexOptions.Compiled);

        public async Task HandleAsync(DotNetParameters parameters)
        {
            // Just POC :) 

            // 1. Check if solution file is the file or directory
            //    if it is null or whitespace we check current directory
            var solutionFile = findSolutionFile.Find(parameters.SolutionFile);

            // 2. Update docker file if exists
            var dockerFiles = solutionFile.Directory!.EnumerateFiles("Dockerfile");

            foreach (var dockerFile in dockerFiles)
            {
                var fileContent = await File.ReadAllTextAsync(dockerFile.FullName).ConfigureAwait(false);

                // FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
                var newfileContent = _versionReplaceRegex.Replace(fileContent, $"{parameters.Version}.0-");
                await File.WriteAllTextAsync(dockerFile.FullName, newfileContent).ConfigureAwait(false);
            }

            // 3. Update directory.props if exists
            var directoryBuildProps = solutionFile.Directory!.EnumerateFiles("Directory.Build.props").FirstOrDefault();

            if (directoryBuildProps.IsNotNull())
            {
                // Read the directoryBuildProps file content and replace net7.0 with net8.0. Can you use a regex that the version can be any number
                var directoryBuildPropsContent = await File.ReadAllTextAsync(directoryBuildProps.FullName).ConfigureAwait(false);
                var newDirectoryBuildPropsContent = _netVersionReplaceRegex.Replace(directoryBuildPropsContent, $"net{parameters.Version}.0");
                await File.WriteAllTextAsync(directoryBuildProps.FullName, newDirectoryBuildPropsContent).ConfigureAwait(false);
            }

            // 4. Update all project files - because we have not already directory.props working in place
            var allProjectFiles = solutionFile.Directory!.EnumerateFiles("*.csproj", SearchOption.AllDirectories);

            foreach (var allProjectFile in allProjectFiles)
            {
                var projectFileContent = await File.ReadAllTextAsync(allProjectFile.FullName).ConfigureAwait(false);
                var newProjectFileContent = _netVersionReplaceRegex.Replace(projectFileContent, $"net{parameters.Version}.0");
                await File.WriteAllTextAsync(allProjectFile.FullName, newProjectFileContent).ConfigureAwait(false);
            }

            consoleService.WriteSuccess($"Solution: {solutionFile.FullName} was successfully migrated to .Net version: {parameters.Version}");
        }
    }
}
