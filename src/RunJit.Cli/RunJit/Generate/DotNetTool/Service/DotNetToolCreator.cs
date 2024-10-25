using System.Collections.Immutable;
using DotNetTool.Service;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.ErrorHandling;
using RunJit.Cli.RunJit.Generate.Client;
using RunJit.Cli.Services;
using RunJit.Cli.Services.Endpoints;
using RunJit.Cli.Services.Net;
using RunJit.Cli.Services.Resharper;
using Solution.Parser.CSharp;
using Solution.Parser.Solution;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    public static class AddDotnetToolGenExtension
    {
        public static void AddDotnetToolGen(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<NetToolGen>();

            services.AddAppCodeGen();
            services.AddAppBuilderCodeGen();
            services.AddErrorHandlerCodeGen();
            services.AddExceptionCodeGen();
            services.AddConsoleServiceCodeGen();
            services.AddProgramCodeGen();
            services.AddStartupCodeGen();
            services.AddCommandCodeGen();
        }
    }

    internal class NetToolGen(IEnumerable<INetToolCodeGen> codeGenerators)
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

    internal class DotNetToolCreator(ControllerParser controllerParser,
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

            var dotnetToolStructure = convertControllerInfos.ConvertTo(mappedToEndpoints, client);

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

            // 5. Code Gen
            await netToolGen.GenerateAsync(dotnetToolProject, dotnetToolStructure).ConfigureAwait(false);

            // Add new net tool project into solution
            // 2. Remove project
            await dotNet.AddProjectToSolutionAsync(clientSolution, dotnetToolProject).ConfigureAwait(false);

            // ----------------------------------------------------------------------------------------------------------------------------------------

            // 9. Check that needed cli tool builder is installed
            var json = dotnetToolStructure.ToJson();
            var file = Path.Combine(Environment.CurrentDirectory, $"{Guid.NewGuid()}.json");
            await File.WriteAllTextAsync(file, json).ConfigureAwait(false);

            await dotNet.RunAsync("dotnet", $"newtool --from-file {file}").ConfigureAwait(false);

            await Task.CompletedTask;
        }
    }

    public static class AddConvertControllerInfosToDotnetToolStructureExtension
    {
        public static void AddConvertControllerInfosToDotnetToolStructure(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<IConvertControllerInfosToDotnetToolStructure, ConvertControllerInfosToDotnetToolStructure>();
        }
    }

    internal interface IConvertControllerInfosToDotnetToolStructure
    {
        DotNetToolInfos ConvertTo(IImmutableList<EndpointGroup> endpointGroups,
                                  DotNetTool client);
    }

    internal class ConvertControllerInfosToDotnetToolStructure : IConvertControllerInfosToDotnetToolStructure
    {
        public DotNetToolInfos ConvertTo(IImmutableList<EndpointGroup> endpointGroups,
                                         DotNetTool client)
        {
            //var projectname = client.ProjectName;
            //var parameterInfo = new CommandInfo(client.DotNetToolName.NormalizedName,
            //                                    client.DotNetToolName.NormalizedName,
            //                                    client.DotNetToolName.NormalizedName,
            //                                    "First try to generate a dotnet tool from an api",
            //                                    null,
            //                                    ImmutableList<OptionInfo>.Empty,

            //                                    )

            //parameterInfo.Value = client.DotNetToolName.NormalizedName;
            //parameterInfo.Name = client.DotNetToolName.NormalizedName;
            //parameterInfo.Description = "First try to generate a dotnet tool from an api";
            //parameterInfo.Options = new List<OptionInfoInfo>();

            //// convert controller infos into dotnet tool structure
            //// endpointGroups are grouped by version
            //foreach (var endpointGroup in endpointGroups)
            //{
            //    var versionCommand = new SubCommand()
            //    {
            //        Name = endpointGroup.Version.Normalized,
            //        Description = endpointGroup.Version.Normalized,
            //        Value = endpointGroup.Version.Normalized,
            //    };

            //    var command = new SubCommand()
            //    {
            //        Name = endpointGroup.GroupName,
            //        Description = endpointGroup.GroupName,
            //        Value = endpointGroup.GroupName,
            //    };

            //    command.SubCommands.Add(versionCommand);

            //    // each domain is a command
            //    foreach (var endoint in endpointGroup.Endpoints)
            //    {
            //        // Temp tool build do not allow exceptions
            //        if (endoint.Name.Contains("Exception", StringComparison.OrdinalIgnoreCase))
            //        {
            //            continue;
            //        }

            //        var endpointCommand = new SubCommand()
            //        {
            //            Name = endoint.SwaggerOperationId,
            //            Description = endoint.SwaggerOperationId,
            //            Value = endoint.SwaggerOperationId,
            //        };

            //        //foreach (var parameter in method.Parameters)
            //        //{
            //        //    subCommand.OptionInfos.Add(new OptionInfo()
            //        //    {
            //        //        Name = parameter.Name,
            //        //        Description = parameter.Name,
            //        //        IsRequired = parameter.IsOptional,
            //        //        Value = parameter.Name,
            //        //        ArgumentInfo = new ArgumentInfo()
            //        //        {
            //        //            Name = parameter.Name,
            //        //            Description = parameter.Name,
            //        //            Type = parameter.Type
            //        //        }
            //        //    });
            //        //}

            //        versionCommand.SubCommands.Add(endpointCommand);
            //    }

            //    parameterInfo.SubCommands.Add(command);
            //}

            //var dotnetToolInfos = new DotNetToolInfos()
            //{
            //    ProjectName = projectname,
            //    DotNetToolName = new DotNetToolName() { Name = client.DotNetToolName.NormalizedName },
            //    ParameterInfo = parameterInfo
            //};

            return new DotNetToolInfos
                   {
                       CommandInfo = new CommandInfo("",
                                                     "",
                                                     "",
                                                     "",
                                                     new ArgumentInfo(string.Empty, string.Empty, string.Empty,
                                                                      string.Empty, string.Empty, string.Empty),
                                                     ImmutableList<OptionInfo>.Empty,
                                                     ImmutableList<CommandInfo>.Empty),
                       DotNetToolName = new DotNetToolName("", ""),
                       ProjectName = "",
                   };
        }
    }
}
