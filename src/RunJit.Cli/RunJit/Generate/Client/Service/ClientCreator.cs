﻿using System.Collections.Immutable;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using Solution.Parser.CSharp;
using Solution.Parser.Solution;
using FileInfo = System.IO.FileInfo;

namespace RunJit.Cli.RunJit.Generate.Client
{
    // Api.Client
    // -> Api
    //    -> Projects 
    //      -> V1
    //        -> Models
    //          -> GetAllProjectsResponse.cs
    //        -> ProjectV1.cs
    //    -> Users
    //      -> V1
    //        -> Models
    //          -> GetUserResponse.cs
    //        -> UserV1.cs
    //      -> V2
    //        -> Models
    //          -> GetUserResponse.cs
    //        -> UserV1.cs
    // -> Client.cs
    // -> ClientFactory.cs
    public record GeneratedClient(IImmutableList<GeneratedFacade> Facades, string SyntaxTree);

    public record GeneratedFacade(
        IImmutableList<GeneratedClientCodeForController> Endpoints,
        string SyntaxTree,
        string Domain,
        string FacadeName);

    public record GeneratedFacades(IImmutableList<GeneratedFacade> ControllerInfos);

    public record GeneratedClientCodeForController(ControllerInfo ControllerInfo, string SyntaxTree, string Domain);

    internal static class AddClientCreatorExtension
    {
        internal static void AddClientCreator(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddApiVersionFinder();
            services.AddControllerParser();
            services.AddClientCreatorForController();
            services.AddDomainFacedBuilder();
            services.AddClientBuilder();
            services.AddClientFactoryBuilder();
            services.AddModelBuilder();
            services.AddSolutionFileModifier();
            services.AddResharperSettingsBuilder();
            services.AddClientStructureWriter();
            services.AddNugetUpdater();
            services.AddAssemblyTypeLoader();
            services.AddApiTypeLoader();
            services.AddTestStructureWriter();
            services.AddRestructureController();

            services.AddSingletonIfNotExists<ClientCreator>();
        }
    }

    internal class ClientCreator(
        ControllerParser controllerParser,
        ClientCreatorForController endpointClientGenerator,
        DomainFacedBuilder domainFacedBuilder,
        ClientBuilder clientBuilder,
        ClientFactoryBuilder clientFactoryBuilder,
        SolutionFileModifier solutionFileModifier,
        ResharperSettingsBuilder resharperSettingsBuilder,
        ClientStructureWriter clientStructureWriter,
        NugetUpdater nugetUpdater,
        ApiTypeLoader apiTypeLoader,
        TestStructureWriter testStructureWriter,
        RestructureController restructureController)
    {
        internal async Task GenerateClientAsync(Client client, FileInfo clientSolution)
        {
            // 0. Add new client projects into existing solution
            await solutionFileModifier.AddProjectsAsync(client.SolutionFileInfo);

            // 1. Parse source solution - Meta level no code parsing here
            var parsedSolution = new SolutionFileInfo(client.SolutionFileInfo.FullName).Parse();
            var parsedClientSolution = new SolutionFileInfo(clientSolution.FullName).Parse();
            var clientProjectName = $"{parsedClientSolution.SolutionFileInfo.FileNameWithoutExtenion}.Client";
            var clientProject = parsedClientSolution.ProductiveProjects.First(p => p.ProjectFileInfo.FileNameWithoutExtenion.EndsWith(clientProjectName));
            var clientTestProject = parsedClientSolution.UnitTestProjects.First(p => p.ProjectFileInfo.FileNameWithoutExtenion.EndsWith($"{clientProjectName}.Test"));
            var clientName = clientProject.ProjectFileInfo.FileNameWithoutExtenion.Replace(".", string.Empty);

            // 2. Parse all C# files
            var allSyntaxTrees = parsedSolution.ProductiveProjects.Where(p => p.ProjectFileInfo.FileNameWithoutExtenion.DoesNotContain(clientProject.ProjectFileInfo.FileNameWithoutExtenion))
                                               .SelectMany(p => p.CSharpFileInfos.Select(c => c.Parse())).ToImmutableList();

            // 3. Get all types which are declared in the API assembly - Need to unique ident the types for client generation.
            var types = apiTypeLoader.GetAllTypesFrom(parsedSolution);

            // 4. Sync nuget packages - New feature we check which packages are predefined in the template
            //   and sync them up to the parent solution in which it will be included
            await nugetUpdater.UpdateAsync(parsedSolution, clientProject).ConfigureAwait(false);
            await nugetUpdater.UpdateAsync(parsedSolution, clientTestProject).ConfigureAwait(false);

            // 5. Get all controllers
            var controllerInfosOrg = controllerParser.ExtractFrom(allSyntaxTrees, types).OrderBy(controller => controller.Name).ToImmutableList();

            // 5.1 New to reorganize the controllers to get the correct domain name
            var controllerInfos = restructureController.Reorganize(controllerInfosOrg);

            // 6. Get project name
            var projectName = clientProject.ProjectFileInfo.Value.NameWithoutExtension();

            // 7. Collect all endpoints
            var allEndpoints = endpointClientGenerator.Create(controllerInfos, projectName, clientName);

            // 8. Create facades for each domain endpoint
            var facades = domainFacedBuilder.BuildFrom(allEndpoints, projectName, clientName);

            // 9. Create client for facades
            var generatedClient = clientBuilder.BuildFor(facades, projectName, clientName);

            // 10. Create client factory for client, specific for siemens usage with HttpRequest's
            var clientFactory = clientFactoryBuilder.BuildFor(projectName, clientName, generatedClient);

            // 11. Setup R# namespace providers.
            var resharperSettings = resharperSettingsBuilder.BuildFrom(generatedClient);

            // 12. Get client folder
            var clientFolder = clientProject.ProjectFileInfo.Value.Directory!;

            // 13. Write client class
            await File.WriteAllTextAsync(Path.Combine(clientFolder.FullName, $"{clientName}.cs"), generatedClient.SyntaxTree);

            // 14. Write client factory class
            await File.WriteAllTextAsync(Path.Combine(clientFolder.FullName, $"{clientName}Factory.cs"), clientFactory);

            // 15. Write R# settings
            await File.WriteAllTextAsync($"{clientProject.ProjectFileInfo.Value.FullName}.DotSettings", resharperSettings);

            // 16. Write facades, domain and version classes
            await clientStructureWriter.WriteFileStructureAsync(generatedClient, clientProject, projectName, clientName);

            // 17.Creates or updates base test structure
            await testStructureWriter.WriteFileStructureAsync(parsedSolution, clientProject, clientTestProject, projectName, clientName);
        }
    }
}
