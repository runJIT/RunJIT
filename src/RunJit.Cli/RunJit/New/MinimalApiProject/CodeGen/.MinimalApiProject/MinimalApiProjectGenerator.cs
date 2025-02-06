using System.Xml.Linq;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.ErrorHandling;
using RunJit.Cli.Generate.DotNetTool;
using RunJit.Cli.New.MinimalApiProject.CodeGen.github;
using RunJit.Cli.Services.Net;
using RunJit.Cli.Services.Resharper;
using Solution.Parser.Solution;

namespace RunJit.Cli.New.MinimalApiProject.CodeGen.MinimalApiProject
{
    internal static class AddMinimalApiProjectGeneratorExtension
    {
        internal static void AddMinimalApiProjectGenerator(this IServiceCollection services)
        {
            // .github code gens
            services.AddGitHubCodeGen();

            // Project level code gens
            services.AddAppSettingsCodeGen();
            services.AddProgramCodeGen();

            services.AddEmbeddedFiles();

            services.AddSingletonIfNotExists<MinimalApiProjectGenerator>();
        }
    }

    internal class MinimalApiProjectGenerator(IDotNet dotNet,
                                              IEnumerable<IMinimalApiProjectSpecificCodeGen> codeGenerators)

    {
        public async Task<FileInfo> GenerateAsync(SolutionFile solutionFile,
                                                  MinimalApiProjectInfos minimalApiProjectInfos)
        {
            var solutionFileInfo = solutionFile.SolutionFileInfo.Value;

            // POC starts here
            // 1. Check if cli project already exists
            //    Depending on new restriction of microsoft we can not just check the .Net.Web.Sdk
            //    so we need to check the implementation
            var dotNetToolProject = solutionFile.ProductiveProjects.FirstOrDefault(p => p.ProjectFileInfo.FileNameWithoutExtenion.ToLowerInvariant() == minimalApiProjectInfos.ProjectName.ToLowerInvariant());

            if (dotNetToolProject.IsNotNull())
            {
                // 1.1 Remove project
                await dotNet.RemoveProjectFromSolutionAsync(solutionFileInfo, dotNetToolProject.ProjectFileInfo.Value).ConfigureAwait(false);

                // 1.2 If exists remove all files
                dotNetToolProject.ProjectFileInfo.Value.Directory?.Delete(true);
            }

            // 2. Create the .net tool folder -> the name of the tool
            var netToolFolder = new DirectoryInfo(Path.Combine(solutionFileInfo.Directory!.FullName, minimalApiProjectInfos.ProjectName));

            if (netToolFolder.Exists)
            {
                netToolFolder.Delete(true);
            }

            // 4. Create new console project
            // dotnet new console --output folder1/folder2/myapp
            var target = Path.Combine(solutionFileInfo.Directory!.FullName, minimalApiProjectInfos.ProjectName);

            await dotNet.RunAsync("dotnet", $"new webapi --name {minimalApiProjectInfos.ProjectName} --output {target} --framework {minimalApiProjectInfos.NetVersion}").ConfigureAwait(false);

            // 5. Get the new created csproj
            var dotnetToolProject = new FileInfo(Path.Combine(target, $"{minimalApiProjectInfos.ProjectName}.csproj"));

            if (dotnetToolProject.NotExists())
            {
                throw new RunJitException($"Expected .NetTool project does not exists. {dotnetToolProject.FullName}");
            }

            // 6. Add required nuget packages into project
            await dotNet.AddNugetPackageAsync(dotnetToolProject.FullName, "Siemens.AspNet.ErrorHandling", "2.1.0").ConfigureAwait(false);
            await dotNet.AddNugetPackageAsync(dotnetToolProject.FullName, "Extensions.Pack", "6.0.3").ConfigureAwait(false);
            await dotNet.AddNugetPackageAsync(dotnetToolProject.FullName, "Amazon.Lambda.AspNetCoreServer.Hosting", "1.7.2").ConfigureAwait(false);

            // 7. Load csproj content to avoid multiple IO write actions to disk which cause io exceptions
            var xdocument = XDocument.Load(dotnetToolProject.FullName);

            // 8. Generate the whole command structure with arguments, options
            foreach (var codeGenerator in codeGenerators)
            {
                await codeGenerator.GenerateAsync(dotnetToolProject, xdocument, minimalApiProjectInfos).ConfigureAwait(false);
            }

            // 9. Save the modified csproj file just once to avoid multiple IO write actions to disk which cause io exceptions
            xdocument.Save(dotnetToolProject.FullName);

            // 10. And at least we add this project into the solution because we want to avoid to many refreshes as possible
            await dotNet.AddProjectToSolutionAsync(solutionFileInfo, dotnetToolProject, "Api").ConfigureAwait(false);

            // 11. Cleanup code to be in sync with target solution settings :)
            // await solutionCodeCleanup.CleanupSolutionAsync(solutionFileInfo).ConfigureAwait(false);

            // 12. Return the created csproj file
            return dotnetToolProject;
        }
    }
}
