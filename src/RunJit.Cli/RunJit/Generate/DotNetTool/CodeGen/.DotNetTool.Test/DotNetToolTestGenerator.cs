using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.ErrorHandling;
using RunJit.Cli.Services.Net;
using Solution.Parser.Solution;

namespace RunJit.Cli.RunJit.Generate.DotNetTool.DotNetTool.Test
{
    internal static class AddDotNetToolTestGeneratorExtension
    {
        internal static void AddDotNetToolTestGenerator(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<DotNetToolTestGenerator>();
        }
    }

    internal class DotNetToolTestGenerator(IDotNet dotNet,
                                           IEnumerable<IDotNetToolTestSpecificCodeGen> codeGenerators)
    {
        internal async Task<FileInfo> GenerateAsync(SolutionFile solutionFile,
                                                    FileInfo netToolProject,
                                                    DotNetToolInfos dotNetToolInfos)
        {
            // 1. Get the solution file info
            var solutionFileInfo = solutionFile.SolutionFileInfo.Value;

            // 1. Expected test project
            var dotNetToolTestProjectName = $"{dotNetToolInfos.ProjectName}.Test";
            var dotNetToolTestProjectFileInfo = new FileInfo(Path.Combine(solutionFileInfo.Directory!.FullName, dotNetToolTestProjectName));

            // 2. Check if cli test project already exists
            var dotNetToolTestProject = solutionFile.UnitTestProjects.FirstOrDefault(p => p.ProjectFileInfo.FileNameWithoutExtenion.ToLowerInvariant() == dotNetToolTestProjectName.ToLowerInvariant());

            // Important if a test project already exists we cant do a lot because some developers
            // maybe already have changed some code which we would override
            if (dotNetToolTestProject.IsNull())
            {
                // Create new net tool test project
                // dotnet new mstest --output folder1/folder2/myapp
                await dotNet.RunAsync("dotnet", $"new mstest --output {dotNetToolTestProjectFileInfo.FullName}").ConfigureAwait(false);
            }
            else
            {
                // if already exists we get the real project file info because if could be csproj location is different as expected.
                // Sample ./src/nettool.test/nettool.test.csproj
                dotNetToolTestProjectFileInfo = dotNetToolTestProject.ProjectFileInfo.Value;

                // Important if the .Net tool will be new generated it gets a new project guid so we have to unlink and relink later
                await dotNet.RemoveProjectReference(netToolProject, dotNetToolTestProjectFileInfo).ConfigureAwait(false);
            }

            // 3. Create the .net tool folder -> the name of the tool
            var netToolFolder = new DirectoryInfo(Path.Combine(dotNetToolTestProjectFileInfo.Directory!.FullName, dotNetToolInfos.NormalizedName));

            if (netToolFolder.Exists.IsFalse())
            {
                netToolFolder.Create();
            }

            // 4. Get the new created csproj
            if (dotNetToolTestProjectFileInfo.NotExists())
            {
                throw new RunJitException($"Expected .NetTool project does not exists. {dotNetToolTestProjectFileInfo.FullName}");
            }

            // 6. Add required nuget packages into project
            await dotNet.AddNugetPackageAsync(dotNetToolTestProjectFileInfo.FullName, "AspNetCore.Simple.MsTest.Sdk", "4.0.10").ConfigureAwait(false);
            
            // 7. Add needed project references
            await dotNet.AddProjectReference(netToolProject, dotNetToolTestProjectFileInfo).ConfigureAwait(false);

            // 8. Generate the whole command structure with arguments, options
            //    We only generate any kind of code of the test project does not exist already
            //    otherwise we override the code from the developers
            if (dotNetToolTestProject.IsNull())
            {
                foreach (var codeGenerator in codeGenerators)
                {
                    await codeGenerator.GenerateAsync(dotNetToolTestProjectFileInfo, dotNetToolInfos).ConfigureAwait(false);
                }
            }


            // 9. And at least we add this project into the solution because we want to avoid to many refreshes as possible
            await dotNet.AddProjectToSolutionAsync(solutionFileInfo, dotNetToolTestProjectFileInfo).ConfigureAwait(false);

            // 10. Return the created test csproj file
            return dotNetToolTestProjectFileInfo;
        }
    }
}
