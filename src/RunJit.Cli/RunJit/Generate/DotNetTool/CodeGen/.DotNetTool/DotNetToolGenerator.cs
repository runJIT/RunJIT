﻿using System.Xml.Linq;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.ErrorHandling;
using RunJit.Cli.Services.Net;
using Solution.Parser.Solution;

namespace RunJit.Cli.Generate.DotNetTool
{
    internal static class AddDotNetToolGeneratorExtension
    {
        internal static void AddDotNetToolGenerator(this IServiceCollection services)
        {
            // App
            services.AddAppCodeGen();
            services.AddAppBuilderCodeGen();

            // Argument
            services.AddArgumentBuilderCodeGen();

            // ErrorHandling
            services.AddErrorHandlerCodeGen();
            services.AddProblemDetailsCodeGen();
            services.AddProblemDetailsExceptionCodeGen();

            // Appsettings
            services.AddAppSettingsCodeGen();

            // Argument
            services.AddArgumentBuilderCodeGen();

            // Services
            services.AddOutputFormatterCodeGen();
            services.AddOutputServiceCodeGen();
            services.AddOutputWriterCodeGen();

            services.AddConsoleServiceCodeGen();
            services.AddProgramCodeGen();
            services.AddStartupCodeGen();
            services.AddCommandCodeGen();
            services.AddArgumentFixerCodeGen();
            services.AddProjectSettingsCodeGen();

            services.AddProjectEmbeddedFilesCodeGen();
            services.AddProjectTypeCodeGen();

            // HttpCallHandlers
            services.AddHttpCallHandlerCodeGen();
            services.AddHttpCallHandlerFactoryCodeGen();
            services.AddHttpClientFactoryCodeGen();
            services.AddHttpRequestMessageBuilderCodeGen();

            // RequestTypeHandling
            services.AddRequestTypeHandleStrategyCodeGen();

            // ResponseTypeHandling
            services.AddByteArrayResponseTypeHandlerCodeGen();
            services.AddFileStreamResponseTypeHandlerCodeGen();
            services.AddJsonResponseTypeHandlerCodeGen();
            services.AddNotOkResponseTypeHandlerCodeGen();
            services.AddResponseTypeHandleStrategyCodeGen();

            services.AddSingletonIfNotExists<DotNetToolGenerator>();
        }
    }

    internal class DotNetToolGenerator(IDotNet dotNet,
                                       IEnumerable<IDotNetToolSpecificCodeGen> codeGenerators)
    {
        public async Task<FileInfo> GenerateAsync(SolutionFile solutionFile,
                                                  DotNetToolInfos dotNetToolInfos)
        {
            var solutionFileInfo = solutionFile.SolutionFileInfo.Value;

            // POC starts here
            // 1. Check if cli project already exists
            //    Depending on new restriction of microsoft we can not just check the .Net.Web.Sdk
            //    so we need to check the implementation
            var dotNetToolProject = solutionFile.ProductiveProjects.FirstOrDefault(p => p.ProjectFileInfo.FileNameWithoutExtenion.ToLowerInvariant() == dotNetToolInfos.ProjectName.ToLowerInvariant());
            if (dotNetToolProject.IsNotNull())
            {
                // 1.1 Remove project
                await dotNet.RemoveProjectFromSolutionAsync(solutionFileInfo, dotNetToolProject.ProjectFileInfo.Value).ConfigureAwait(false);

                // 1.2 If exists remove all files
                dotNetToolProject.ProjectFileInfo.Value.Directory?.Delete(true);
            }

            // 2. Create the .net tool folder -> the name of the tool
            var netToolFolder = new DirectoryInfo(Path.Combine(solutionFileInfo.Directory!.FullName, dotNetToolInfos.ProjectName));
            if (netToolFolder.Exists)
            {
                netToolFolder.Delete(true);
            }

            // 4. Create new console project
            // dotnet new console --output folder1/folder2/myapp
            var target = Path.Combine(solutionFileInfo.Directory!.FullName, dotNetToolInfos.ProjectName);
            await dotNet.RunAsync("dotnet", $"new console --output {target}").ConfigureAwait(false);

            // 5. Get the new created csproj
            var dotnetToolProject = new FileInfo(Path.Combine(target, $"{dotNetToolInfos.ProjectName}.csproj"));
            if (dotnetToolProject.NotExists())
            {
                throw new RunJitException($"Expected .NetTool project does not exists. {dotnetToolProject.FullName}");
            }

            // 6. Add required nuget packages into project
            await dotNet.AddNugetPackageAsync(dotnetToolProject.FullName, "System.CommandLine", "0.3.0-alpha.20054.1").ConfigureAwait(false);
            await dotNet.AddNugetPackageAsync(dotnetToolProject.FullName, "Extensions.Pack", "5.0.4").ConfigureAwait(false);
            await dotNet.AddNugetPackageAsync(dotnetToolProject.FullName, "GitVersion.MsBuild", "6.0.3").ConfigureAwait(false);
            await dotNet.AddNugetPackageAsync(dotnetToolProject.FullName, "Microsoft.Extensions.DependencyInjection", "8.0.1").ConfigureAwait(false);
            await dotNet.AddNugetPackageAsync(dotnetToolProject.FullName, "Microsoft.Extensions.Configuration", "8.0.0").ConfigureAwait(false);
            await dotNet.AddNugetPackageAsync(dotnetToolProject.FullName, "Microsoft.Extensions.Configuration.EnvironmentVariables", "8.0.0").ConfigureAwait(false);
            await dotNet.AddNugetPackageAsync(dotnetToolProject.FullName, "Microsoft.Extensions.Configuration.UserSecrets", "8.0.0").ConfigureAwait(false);

            // 7. Load csproj content to avoid multiple IO write actions to disk which cause io exceptions
            var xdocument = XDocument.Load(dotnetToolProject.FullName);

            // 8. Generate the whole command structure with arguments, options
            foreach (var codeGenerator in codeGenerators)
            {
                await codeGenerator.GenerateAsync(dotnetToolProject, xdocument, dotNetToolInfos).ConfigureAwait(false);
            }

            // 9. Save the modified csproj file just once to avoid multiple IO write actions to disk which cause io exceptions
            xdocument.Save(dotnetToolProject.FullName);

            // 10. And at least we add this project into the solution because we want to avoid to many refreshes as possible
            await dotNet.AddProjectToSolutionAsync(solutionFileInfo, dotnetToolProject).ConfigureAwait(false);

            // 11. Return the created csproj file
            return dotnetToolProject;
        }
    }
}
