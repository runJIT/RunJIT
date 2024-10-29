using DotNetTool.Service;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using Solution.Parser.Project;
using Solution.Parser.Solution;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal static class AddTestStructureWriterExtension
    {
        internal static void AddTestStructureWriter(this IServiceCollection services)
        {
            services.AddMsTestBaseClassBuilder();
            services.AddAppsettingsBuilder();

            services.AddSingletonIfNotExists<TestStructureWriter>();
        }
    }

    internal sealed class TestStructureWriter(MsTestBaseClassBuilder msTestBaseClassBuilder,
                                       AppsettingsBuilder appsettingsBuilder)
    {
        public async Task WriteFileStructureAsync(SolutionFile solutionFile,
                                                  Solution.Parser.Project.ProjectFile clientProject,
                                                  Solution.Parser.Project.ProjectFile clientTestProject,
                                                  string projectName,
                                                  string clientName)
        {
            // 1. Environment folder
            // Test structure:
            // Environment
            // -> MsTestBase
            var environmentFolder = new DirectoryInfo(Path.Combine(clientTestProject.ProjectFileInfo.Value.Directory!.FullName, "Environment"));

            if (environmentFolder.NotExists())
            {
                environmentFolder.Create();
            }

            // 2. MsTest base class
            var msTestBaseClass = msTestBaseClassBuilder.BuildFor(clientTestProject.ProjectFileInfo.Value.NameWithoutExtension(), clientName);
            var msTestBaseFile = new FileInfo(Path.Combine(environmentFolder.FullName, "MsTestBase.cs"));

            // 2.1 Important we will not overwrite if it exists already because of custom changes
            if (msTestBaseFile.NotExists())
            {
                await File.WriteAllTextAsync(msTestBaseFile.FullName, msTestBaseClass).ConfigureAwait(false);
            }

            // 3. appsettings.test.json
            var appSettings = appsettingsBuilder.BuildFor(projectName, clientName);
            var appSettingsFile = new FileInfo(Path.Combine(clientTestProject.ProjectFileInfo.Value.Directory!.FullName, "appsettings.test.json"));

            // 3.1 Important we will not overwrite if it exists already because of custom changes
            if (appSettingsFile.NotExists())
            {
                await File.WriteAllTextAsync(appSettingsFile.FullName, appSettings).ConfigureAwait(false);
            }

            // 4. Add project references
            var dotNetTool = DotNetToolFactory.Create();

            // 4.1 DotNetTool project reference is needed
            await dotNetTool.RunAsync("dotnet", $"add {clientTestProject.ProjectFileInfo.Value.FullName} reference {clientProject.ProjectFileInfo.Value.FullName}");

            // 4.2 API project reference is needed too because of startup.cs
            var webAppProject = solutionFile.ProductiveProjects.FirstOrDefault(p => p.Document.ToString().Contains("Sdk=\"Microsoft.NET.Sdk.Web\""));

            if (webAppProject.IsNotNull())
            {
                await dotNetTool.RunAsync("dotnet", $"add {clientTestProject.ProjectFileInfo.Value.FullName} reference {webAppProject.ProjectFileInfo.Value.FullName}");
            }
        }
    }
}
