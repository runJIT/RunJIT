using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;
using Solution.Parser.Solution;

namespace RunJit.Cli.New.MinimalApiProject.CodeGen
{
    internal static class AddDirectoryBuildPropsCodeGenExtension
    {
        internal static void AddDirectoryBuildPropsCodeGen(this IServiceCollection services)
        {
            services.AddConsoleService();
            services.AddNamespaceProvider();

            services.AddSingletonIfNotExists<IMinimalApiProjectRootLevelCodeGen, DirectoryBuildPropsCodeGen>();
        }
    }

    internal sealed class DirectoryBuildPropsCodeGen(ConsoleService consoleService) : IMinimalApiProjectRootLevelCodeGen
    {
        private const string Template = """
                                        <Project>
                                            <PropertyGroup>
                                                <!-- Sets the target framework for the project to .NET 8.0 -->
                                                <TargetFramework>net8.0</TargetFramework>
                                        
                                                <!-- Sets the C# language version to the latest major version -->
                                                <LangVersion>latestmajor</LangVersion>
                                        
                                                <!-- Enables .NET analyzers which provide code quality analysis -->
                                                <EnableNETAnalyzers>true</EnableNETAnalyzers>
                                        
                                                <!-- Sets the analysis level to the latest to use the most up-to-date rules and improvements -->
                                                <AnalysisLevel>latest</AnalysisLevel>
                                        
                                                <!-- Enables nullable reference types to avoid null reference errors -->
                                                <Nullable>enable</Nullable>
                                        
                                                <!-- Treats all warnings related to nullable reference types as errors -->
                                                <WarningsAsErrors>Nullable</WarningsAsErrors>
                                        
                                                <!-- Enforces code style rules during the build process -->
                                                <EnforceCodeStyleInBuild>true</EnforceCodeStyleInBuild>
                                        
                                                <!-- Treats all warnings as errors to ensure code is warning-free -->
                                                <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
                                        
                                                <!-- Accelerates the build process in Visual Studio -->
                                                <AccelerateBuildsInVisualStudio>true</AccelerateBuildsInVisualStudio>
                                        
                                                <!-- Generates an XML documentation file for the code, which is imporant for libaries api's and more -->
                                                <GenerateDocumentationFile>true</GenerateDocumentationFile>
                                        
                                                <ImplicitUsings>enable</ImplicitUsings>
                                                
                                                <InvariantGlobalization>true</InvariantGlobalization>
                                            </PropertyGroup>
                                        </Project>
                                        """;


        public async Task GenerateAsync(SolutionFile solutionFile,
                                        MinimalApiProjectInfos minimalApiProjectInfos)
        {
            // 1 Setup file name
            var file = Path.Combine(solutionFile.SolutionFileInfo.Value.Directory!.FullName, "Directory.Build.props");


            await File.WriteAllTextAsync(file, Template).ConfigureAwait(false);

            // 3. Print success message
            consoleService.WriteSuccess($"Successfully created {file}");
        }
    }
}
