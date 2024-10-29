﻿using System.Collections.Immutable;
using DotNetTool.Service;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Api.Client.RequestTypeHandling;
using RunJit.Cli.ErrorHandling;
using RunJit.Cli.RunJit.Generate.Client;
using RunJit.Cli.RunJit.Generate.DotNetTool.AppSettings;
using RunJit.Cli.Services;
using RunJit.Cli.Services.Endpoints;
using RunJit.Cli.Services.Net;
using RunJit.Cli.Services.Resharper;
using Solution.Parser.CSharp;
using Solution.Parser.Solution;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal static class AddDotnetToolGenExtension
    {
        internal static void AddDotnetToolGen(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<NetToolGen>();

            // App
            services.AddAppCodeGen();
            services.AddAppBuilderCodeGen();

            // ErrorHandling
            services.AddErrorHandlerCodeGen();
            services.AddProblemDetailsCodeGen();
            services.AddProblemDetailsExceptionCodeGen();


            // Appsettings
            services.AddAppSettingsCodeGen();

            // Argument
            services.AddArgumentBuilderCodeGen();

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
        }
    }

    internal sealed class NetToolGen(IEnumerable<INetToolCodeGen> codeGenerators)
    {
        internal async Task GenerateAsync(FileInfo projectFileInfo,
                                          DotNetToolInfos dotNetToolInfos)
        {
            foreach (var codeGenerator in codeGenerators)
            {
                await codeGenerator.GenerateAsync(projectFileInfo, dotNetToolInfos).ConfigureAwait(false);
            }
        }
    }

    // Pulse.Sustainability.DotNetTool
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
    // -> PulseSustainabilityDotNetTool.cs
    // -> PulseSustainabilityDotNetToolFactory.cs
    public record GeneratedDotNetTool(IImmutableList<GeneratedFacade> Facades,
                                      string SyntaxTree);

    public record GeneratedFacade(IImmutableList<GeneratedDotNetToolCodeForController> Endpoints,
                                  string SyntaxTree,
                                  string Domain,
                                  string FacadeName);

    public record GeneratedDotNetToolCodeForController(ControllerInfo ControllerInfo,
                                                       string SyntaxTree,
                                                       string Domain);

    internal static class AddDotNetToolCreatorExtension
    {
        internal static void AddDotNetToolCreator(this IServiceCollection services)
        {
            services.AddApiVersionFinder();
            services.AddControllerParser();
            services.AddDotNetToolCreatorForController();
            services.AddDomainFacedBuilder();
            services.AddDotNetToolBuilder();
            services.AddDotNetToolFactoryBuilder();
            services.AddModelBuilder();
            services.AddSolutionFileModifier();
            services.AddResharperSettingsBuilder();

            services.AddNugetUpdater();
            services.AddAssemblyTypeLoader();
            services.AddApiTypeLoader();
            services.AddTestStructureWriter();
            services.AddRestructureController();
            services.AddConvertControllerInfosToDotnetToolStructure();

            services.AddDotnetToolGen();

            services.AddSingletonIfNotExists<DotNetToolCreator>();
        }
    }

    internal sealed class DotNetToolCreator(ControllerParser controllerParser,
                                     ApiTypeLoader apiTypeLoader,
                                     RestructureController restructureController,
                                     IDotNet dotNet,
                                     IConvertControllerInfosToDotnetToolStructure convertControllerInfos,
                                     MinimalApiEndpointParser minimalApiEndpointParser,
                                     OrganizeMinimalEndpoints organizeMinimalEndpoints,
                                     NetToolGen netToolGen)
    {
        internal async Task GenerateDotNetToolAsync(DotNetTool client,
                                                    FileInfo clientSolution)
        {
            // 0. Pre requirements
            var dotnetTool = DotNetToolFactory.Create();
            var dotnettoolBuilder = "DotNetTool.Builder";

            var dotnetToolBuilderExists = await dotnetTool.ExistsAsync(dotnettoolBuilder).ConfigureAwait(false);

            if (dotnetToolBuilderExists.IsNull())
            {
                var installResult = await dotnetTool.InstallAsync(dotnettoolBuilder).ConfigureAwait(false);

                if (installResult.ExitCode != 0)
                {
                    throw new RunJitException($"Could not install {dotnettoolBuilder}");
                }
            }

            // 4. Create new project
            // 5. Add all helper classes
            //    App
            //    - App
            //    - AppBuilder
            //    ErrorHandling
            //    - ErrorHandler
            //    - Exception
            //    Services
            //    - ConsoleService
            //    MyTool (The cli which have to be generated)
            //    - Users
            //      - V1
            //        - Add
            //    Program.cs
            //    Startup.cs

            // 1. Build the target solution first
            //await dotNet.BuildAsync(clientSolution).ConfigureAwait(false);

            // 2. Parse the solution
            var parsedSolution = new SolutionFileInfo(clientSolution.FullName).Parse();

            // 3. Parse all C# files
            var allSyntaxTrees = parsedSolution.ProductiveProjects.SelectMany(p => p.CSharpFileInfos.Select(c => c.Parse())).ToImmutableList();

            // 4. Get all types which are declared in the API assembly - Need to unique ident the types for client generation.
            var types = apiTypeLoader.GetAllTypesFrom(parsedSolution);

            // 5.1 New to reorganize the controllers to get the correct domain name
            // 5. Get all controllers
            var controllerInfosOrg = controllerParser.ExtractFrom(allSyntaxTrees, types).OrderBy(controller => controller.Name).ToImmutableList();

            // 6.1 New to reorganize the controllers to get the correct domain name
            var controllerInfos = restructureController.Reorganize(controllerInfosOrg);

            // 7.2 Get all minimal endpoints
            var endpoints = minimalApiEndpointParser.ExtractFrom(allSyntaxTrees, types).OrderBy(controller => controller.Name).ToImmutableList();

            // 8.3 organize endpoints
            var organizedEndpoints = organizeMinimalEndpoints.Reorganize(endpoints);

            // NEW !! TEST POC
            var mappedToEndpoints = organizedEndpoints.Any() ? organizedEndpoints : controllerInfos.ToEndpointInfos();

            // We wan to have them grouped by domain to get correct tree like:
            // - Users (1x)
            //   - V1  (1x)
            //   - V2  (1x)
            var domainGrouped = mappedToEndpoints.GroupBy(e => e.GroupName).Distinct().ToImmutableList();

            var dotnetToolStructure = convertControllerInfos.ConvertTo(domainGrouped, client);

            // ----------------------------------------------------------------------------------------------------------------------------------------
            // POC starts here
            // 1. Check if cli project already exists
            var dotNetToolProject = parsedSolution.ProductiveProjects.FirstOrDefault(p => p.ProjectFileInfo.FileNameWithoutExtenion == client.ProjectName);

            if (dotNetToolProject.IsNotNull())
            {
                // 2. Remove project
                await dotNet.RemoveProjectFromSolutionAsync(clientSolution, dotNetToolProject.ProjectFileInfo.Value).ConfigureAwait(false);

                // 3. If exists remove all files
                dotNetToolProject.ProjectFileInfo.Value.Directory?.Delete(true);
            }

            var netToolFolder = new DirectoryInfo(Path.Combine(clientSolution.Directory!.FullName, client.ProjectName));

            if (netToolFolder.Exists)
            {
                netToolFolder.Delete(true);
            }

            // 4. Create new project
            // dotnet new console --output folder1/folder2/myapp
            var target = Path.Combine(clientSolution.Directory!.FullName, client.ProjectName);
            await dotNet.RunAsync("dotnet", $"new console --output {target}").ConfigureAwait(false);

            var dotnetToolProject = new FileInfo(Path.Combine(target, $"{client.ProjectName}.csproj"));
            if (dotnetToolProject.NotExists())
            {
                throw new RunJitException($"Expected .NetTool project does not exists. {dotnetToolProject.FullName}");
            }

            // Add required nuget packages into project
            await dotNet.AddNugetPackageAsync(dotnetToolProject.FullName, "System.CommandLine", "0.3.0-alpha.20054.1").ConfigureAwait(false);
            await dotNet.AddNugetPackageAsync(dotnetToolProject.FullName, "Extensions.Pack", "5.0.4").ConfigureAwait(false);
            await dotNet.AddNugetPackageAsync(dotnetToolProject.FullName, "GitVersion.MsBuild", "6.0.3").ConfigureAwait(false);
            await dotNet.AddNugetPackageAsync(dotnetToolProject.FullName, "Microsoft.Extensions.DependencyInjection", "8.0.1").ConfigureAwait(false);
            await dotNet.AddNugetPackageAsync(dotnetToolProject.FullName, "Microsoft.Extensions.Configuration", "8.0.0").ConfigureAwait(false);
            await dotNet.AddNugetPackageAsync(dotnetToolProject.FullName, "Microsoft.Extensions.Configuration.EnvironmentVariables", "8.0.0").ConfigureAwait(false);
            await dotNet.AddNugetPackageAsync(dotnetToolProject.FullName, "Microsoft.Extensions.Configuration.UserSecrets", "8.0.0").ConfigureAwait(false);
            await dotNet.AddNugetPackageAsync(dotnetToolProject.FullName, "Siemens.AspNet.ErrorHandling.Contracts", "1.0.0").ConfigureAwait(false);

            // 5. Code Gen
            await netToolGen.GenerateAsync(dotnetToolProject, dotnetToolStructure).ConfigureAwait(false);

            // Add new net tool project into solution
            await dotNet.AddProjectToSolutionAsync(clientSolution, dotnetToolProject).ConfigureAwait(false);
        }
    }

    internal static class AddConvertControllerInfosToDotnetToolStructureExtension
    {
        internal static void AddConvertControllerInfosToDotnetToolStructure(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<IConvertControllerInfosToDotnetToolStructure, ConvertControllerInfosToDotnetToolStructure>();
        }
    }

    internal interface IConvertControllerInfosToDotnetToolStructure
    {
        DotNetToolInfos ConvertTo(ImmutableList<IGrouping<string, EndpointGroup>> domainGroupedByVersion,
                                  DotNetTool client);
    }

    internal sealed class ConvertControllerInfosToDotnetToolStructure : IConvertControllerInfosToDotnetToolStructure
    {
        public DotNetToolInfos ConvertTo(ImmutableList<IGrouping<string, EndpointGroup>> domainGroupedByVersion,
                                         DotNetTool client)
        {
            var projectname = client.ProjectName;

            // Root command is the first command
            // Sample:
            // - dotnet tool install (dotnet is root command)
            // - pulse core resources v1 (pulse is the root command)
            var rootCommand = new CommandInfo
            {
                NormalizedName = client.DotNetToolName.NormalizedName,
                Value = client.DotNetToolName.Name,
                Name = client.DotNetToolName.Name,
                Description = client.DotNetToolName.Name,
            };


            // For your tool we want to be able to manage the whole configuration
            // So we need a command to get/set the configuration
            var configCommand = new CommandInfo
            {
                NormalizedName = "config",
                Value = "config",
                Name = "config",
                Description = "Command to handle the configuration for your dotnet tool. Known as appsettings",
            };

            // We need a command to read the whole config
            var getConfigCommand = new CommandInfo()
                                   {
                                       NormalizedName = "get",
                                       Value = "get",
                                       Name = "get",
                                       Description = "Command to read the whole configuration for your dotnet tool. Known as appsettings",
                                   };

            // We need a command to set/update the whole config
            var setConfigCommand = new CommandInfo()
                                   {
                                       NormalizedName = "set",
                                       Value = "set",
                                       Name = "set",
                                       Description = "Command to set the whole configuration for your dotnet tool. Known as appsettings. To set the configuration call 'configuration get' change the settings you want to change and call 'configuration set' to set the whole json back",
                                   };

            configCommand.SubCommands.Add(getConfigCommand);
            configCommand.SubCommands.Add(setConfigCommand);

            // convert controller infos into dotnet tool structure
            // endpointGroups are grouped by version
            foreach (var domainGroupByVersion in domainGroupedByVersion)
            {
                // Domain command like
                // Users
                // Resources
                var domainCommand = new CommandInfo()
                {
                    Name = domainGroupByVersion.Key,
                    NormalizedName = domainGroupByVersion.Key,
                    Description = $"Here comes the description for {domainGroupByVersion.Key}",
                    Value = domainGroupByVersion.Key,
                };

                foreach (var version in domainGroupByVersion)
                {
                    // Domain command like
                    // Users
                    //  - V1
                    // Resources
                    //  - V1
                    var versionCommand = new CommandInfo()
                    {
                        Name = version.Version.Normalized,
                        NormalizedName = version.Version.Normalized,
                        Description = $"Here comes the description for {version.Version.Normalized}",
                        Value = version.Version.Normalized,
                    };

                    domainCommand.SubCommands.Add(versionCommand);


                    // each domain is a command
                    foreach (var endoint in version.Endpoints)
                    {
                        // Domain command like
                        // Users
                        //  - V1
                        //    - AddUser
                        // Resources
                        //  - V1
                        //    - AddResource
                        // Temp tool build do not allow exceptions
                        var endpointCommand = new CommandInfo()
                        {
                            Name = endoint.SwaggerOperationId.FirstCharToUpper(),
                            NormalizedName = endoint.SwaggerOperationId.FirstCharToUpper(),
                            Description = $"Here comes the description for {endoint.SwaggerOperationId.FirstCharToUpper()}",
                            Value = endoint.SwaggerOperationId.FirstCharToUpper(),
                            Argument = endoint.Parameters.IsEmpty() ? null : new ArgumentInfo("json", "Call info as json which contains url- and query parameters as well the payload if needed", "<callInfos>[string]", "string", "string", "Json"),
                            EndpointInfo = endoint,
                            //MethodBody = """
                            //             var result =  await _httpCallHandler.CallAsync<Todo>(HttpMethod.Post, "api/v1.0/todos", todo, CancellationToken.None).ConfigureAwait(false);
                            //             consoleService.WriteSuccess(result);
                            //             """,
                            Options = new List<OptionInfo>()
                                      {
                                          new OptionInfo()
                                          {
                                              Alias = "-t",
                                              NormalizedName = "token",
                                              Argument = new ArgumentInfo("token", "Bearer token for authentication", "<token>[string]", "string", "string", "Token"),
                                              IsIsRequired = false,
                                              Name = "token",
                                              Value = "--token",
                                              Description = "Bearer token for authentication"
                                          }
                                      }
                            
                        };


                        versionCommand.SubCommands.Add(endpointCommand);
                    }
                }

                rootCommand.SubCommands.Add(domainCommand);
            }

            var dotnetToolInfos = new DotNetToolInfos()
            {
                ProjectName = projectname,
                DotNetToolName = client.DotNetToolName,
                CommandInfo = rootCommand
            };

            return dotnetToolInfos;
        }
    }
}
