using System.Collections.Immutable;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.ErrorHandling;
using RunJit.Cli.RunJit.Update.Net;
using RunJit.Cli.Services;
using RunJit.Cli.Services.Git;
using RunJit.Cli.Services.Net;
using Solution.Parser.Solution;

namespace RunJit.Cli.RunJit.Update.SwaggerTests
{
    internal static class AddUpdateLocalSolutionFileExtension
    {
        internal static void AddUpdateLocalSolutionFile(this IServiceCollection services)
        {
            services.AddConsoleService();
            services.AddGitService();
            services.AddDotNet();
            services.AddDotNetService();

            // services.AddUpdateSwaggerTestsPackageService();
            services.AddFindSolutionFile();

            services.AddSingletonIfNotExists<IUpdateSwaggerTestsStrategy, UpdateLocalSolutionFile>();
        }
    }

    internal class UpdateLocalSolutionFile(IConsoleService consoleService,
                                           IDotNet dotNet,
                                           EmbeddedFileService embeddedFileService,
                                           FindSolutionFile findSolutionFile) : IUpdateSwaggerTestsStrategy
    {
        public bool CanHandle(UpdateSwaggerTestsParameters parameters)
        {
            return parameters.SolutionFile.IsNotNullOrWhiteSpace();
        }

        public async Task HandleAsync(UpdateSwaggerTestsParameters parameters)
        {
            // 0. Check that precondition is met
            if (CanHandle(parameters).IsFalse())
            {
                throw new RunJitException($"Please call {nameof(IUpdateSwaggerTestsStrategy.CanHandle)} before call {nameof(IUpdateSwaggerTestsStrategy.HandleAsync)}");
            }

            // 5. Check if solution file is the file or directory
            //    if it is null or whitespace we check current directory 
            var solutionFile = findSolutionFile.Find(Environment.CurrentDirectory);

            // 6. Build the solution first
            await dotNet.BuildAsync(solutionFile).ConfigureAwait(false);

            var solutionFileParsed = new SolutionFileInfo(solutionFile.FullName).Parse();

            var webApiProject = solutionFileParsed.ProductiveProjects.Where(p => p.Document.ToString().Contains("Sdk=\"Microsoft.NET.Sdk.Web\"")).ToImmutableList();

            if (webApiProject.Count != 1)
            {
                throw new RunJitException($"Could not find a web api project in solution: {solutionFile.FullName}");
            }

            var targetTestProject = solutionFileParsed.UnitTestProjects.FirstOrDefault(p => p.ProjectFileInfo.FileNameWithoutExtenion.Contains($"{webApiProject[0].ProjectFileInfo.FileNameWithoutExtenion}.Test"));

            if (targetTestProject.IsNull())
            {
                throw new RunJitException($"Could not find the test project for the web api in the solution: {solutionFile.FullName}");
            }

            // Go sure solution parser is in place
            // Now we start Magic
            var solutionParser = targetTestProject.PackageReferences.FirstOrDefault(p => p.Include.Contains("Solution.Parser"));

            if (solutionParser.IsNull())
            {
                await dotNet.RunAsync("dotnet", $"add {targetTestProject.ProjectFileInfo.Value.FullName} package Solution.Parser").ConfigureAwait(false);
            }

            // JsonDiffPatch
            var jsonDiffPatch = targetTestProject.PackageReferences.FirstOrDefault(p => p.Include.Contains("JsonDiffPatch"));

            if (jsonDiffPatch.IsNull())
            {
                await dotNet.RunAsync("dotnet", $"add {targetTestProject.ProjectFileInfo.Value.FullName} package JsonDiffPatch").ConfigureAwait(false);
            }

            var targetPath = Path.Combine(targetTestProject.ProjectFileInfo.Value.Directory!.FullName, "SwaggerInitTest.cs");
            var fileContent = EmbeddedFile.GetFileContentFrom("RunJit.Update.SwaggerTests.Templates.SwaggerTestInitializer.rps");
            var nameWithoutExtension = solutionFile.NameWithoutExtension().Split('.').Select(c => c.FirstCharToUpper()).Flatten(".");
            var withoutExtension = webApiProject[0].ProjectFileInfo.Value.NameWithoutExtension().Split('.').Select(c => c.FirstCharToUpper()).Flatten(".");

            fileContent = fileContent.Replace("$solutionName$", nameWithoutExtension)
                                     .Replace("$webApiProject$", withoutExtension);

            await File.WriteAllTextAsync(targetPath, fileContent).ConfigureAwait(false);

            //// Now we start Magic
            await dotNet.RunAsync("dotnet", "test --filter TestCategory=SwaggerInitializer").ConfigureAwait(false);

            // Now we have to embed the files if they was not already embedded
            var swaggerFolder = new DirectoryInfo(Path.Combine(targetTestProject.ProjectFileInfo.Value.Directory!.FullName, "Swagger"));
            var jsonFiles = swaggerFolder.EnumerateFiles("*.json", SearchOption.AllDirectories).ToImmutableList();

            // 7. Embed the JSON files into the test project
            foreach (var jsonFile in jsonFiles)
            {
                var relativePath = jsonFile.FullName.Replace(targetTestProject.ProjectFileInfo.Value.Directory.Parent!.FullName, string.Empty).TrimStart('\\').Replace(@"\", ".").Replace(jsonFile.Name, string.Empty).TrimEnd('.');
                embeddedFileService.EmbedFile(targetTestProject, relativePath, jsonFile.Name);
            }

            //// Now we delete the file :) like magic
            File.Delete(targetPath);

            consoleService.WriteSuccess($"Solution: {solutionFile.FullName} was successfully update to the newest swagger tests");
        }
    }
}
