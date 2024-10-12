using System.Collections.Immutable;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Net;
using RunJit.Cli.RunJit.Generate.DotNetTool.CodeBuilders;
using Solution.Parser.CSharp;
using Solution.Parser.Solution;
using FileInfo = System.IO.FileInfo;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
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
    public record GeneratedDotNetTool(IImmutableList<GeneratedFacade> Facades, string SyntaxTree);

    public record GeneratedFacade(
        IImmutableList<GeneratedDotNetToolCodeForController> Endpoints,
        string SyntaxTree,
        string Domain,
        string FacadeName);

    
    public record GeneratedDotNetToolCodeForController(ControllerInfo ControllerInfo, string SyntaxTree, string Domain);

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

            services.AddSingletonIfNotExists<DotNetToolCreator>();
        }
    }

    internal class DotNetToolCreator(ControllerParser controllerParser,
                                     ApiTypeLoader apiTypeLoader,
                                     RestructureController restructureController,
                                     IDotNet dotNet,
                                     IConvertControllerInfosToDotnetToolStructure convertControllerInfos)
    {
        internal async Task GenerateDotNetToolAsync(DotNetTool client, FileInfo clientSolution)
        {
            await dotNet.BuildAsync(clientSolution).ConfigureAwait(false);

            var parsedSolution = new SolutionFileInfo(clientSolution.FullName).Parse();

            // 2. Parse all C# files
            var allSyntaxTrees = parsedSolution.ProductiveProjects.SelectMany(p => p.CSharpFileInfos.Select(c => c.Parse())).ToImmutableList();

            // 3. Get all types which are declared in the API assembly - Need to unique ident the types for client generation.
            var types = apiTypeLoader.GetAllTypesFrom(parsedSolution);

            // 5. Get all controllers
            var controllerInfosOrg = controllerParser.ExtractFrom(allSyntaxTrees, types).OrderBy(controller => controller.Name).ToImmutableList();

            // 5.1 New to reorganize the controllers to get the correct domain name
            var controllerInfos = restructureController.Reorganize(controllerInfosOrg);
            
            var dotnetToolStructure = convertControllerInfos.ConvertTo(controllerInfos, client);

            // 6. Check that needed cli tool builder is installed
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
        ConvertControllerInfosToDotnetToolStructure.DotNetToolInfos ConvertTo(ImmutableList<ControllerInfo> controllerInfos, DotNetTool client);
    }

    internal class ConvertControllerInfosToDotnetToolStructure : IConvertControllerInfosToDotnetToolStructure
    {
        public DotNetToolInfos ConvertTo(ImmutableList<ControllerInfo> controllerInfos, DotNetTool client)
        {
            var projectname = client.ProjectName.Split(".").First().ToLowerInvariant();
            var parameterInfo = new ParameterInfo();

            parameterInfo.Value = projectname;
            parameterInfo.Name = projectname;
            parameterInfo.Description = "First try to generate a dotnet tool from an api";
            parameterInfo.Options = new List<OptionInfoInfo>();

            // convert controller infos into dotnet tool structure
            foreach (var controllerInfo in controllerInfos)
            {
                if (controllerInfo.Version.Normalized != "V1")
                {
                    continue;
                }
                
                if (controllerInfo.Name.Contains("Filter", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var command = new SubCommand()
                {
                    Name = controllerInfo.Name,
                    Description = controllerInfo.Name,
                    Value = controllerInfo.Name,
                };

                // each domain is a command
                foreach (var method in controllerInfo.Methods)
                {
                    // Temp tool build do not allow exceptions
                    if (method.Name.Contains("Exception", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    var subCommand = new SubCommand()
                    {
                        Name = method.Name,
                        Description = method.Name,
                        Value = method.Name,
                    };

                    //foreach (var parameter in method.Parameters)
                    //{
                    //    subCommand.OptionInfos.Add(new OptionInfo()
                    //    {
                    //        Name = parameter.Name,
                    //        Description = parameter.Name,
                    //        IsRequired = parameter.IsOptional,
                    //        Value = parameter.Name,
                    //        ArgumentInfo = new ArgumentInfo()
                    //        {
                    //            Name = parameter.Name,
                    //            Description = parameter.Name,
                    //            Type = parameter.Type
                    //        }
                    //    });
                    //}

                    command.SubCommands.Add(subCommand);
                }

                parameterInfo.SubCommands.Add(command);
            }


            var dotnetToolInfos = new DotNetToolInfos()
            {
                ProjectName = projectname,
                DotNetToolName = new DotNetToolName()
                {
                    Name = projectname
                },
                ParameterInfo = parameterInfo
            };

            return dotnetToolInfos;
        }

        internal record ArgumentInfo
        {
            public string Description { get; init; } = string.Empty;
            public string Type { get; init; } = string.Empty;
            public string Name { get; init; } = string.Empty;
        }

        internal record DotNetToolName
        {
            public string Name { get; init; } = string.Empty;
        }

        internal record OptionInfo
        {
            public string Alias { get; init; } = string.Empty;
            public string Description { get; init; } = string.Empty;
            public bool IsRequired { get; init; }
            public ArgumentInfo? Argument { get; init; } = null;
            public string ArgumentInfoName { get; init; } = string.Empty;
            public string Value { get; init; } = string.Empty;
            public string Name { get; init; } = string.Empty;
        }

        internal class ParameterInfo
        {
            public List<SubCommand> SubCommands { get; init; } = new List<SubCommand>();
            public List<OptionInfoInfo> Options { get; set; } = new List<OptionInfoInfo>();
            public ArgumentInfo? Argument { get; init; } = null;
            public string Description { get; set; } = string.Empty;
            public string Value { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
        }

        internal record DotNetToolInfos
        {
            public string ProjectName { get; init; } = string.Empty;
            public required DotNetToolName DotNetToolName { get; init; }
            public required ParameterInfo ParameterInfo { get; init; }
        }

        internal record SubCommand
        {
            public List<SubCommand> SubCommands { get; init; } = new List<SubCommand>();
            public List<OptionInfo> Options { get; init; } = new List<OptionInfo>();
            public ArgumentInfo? Argument { get; init; } = null;
            public string Description { get; init; } = string.Empty;
            public string Value { get; init; } = string.Empty;
            public string Name { get; init; } = string.Empty;
        }

        internal record OptionInfoInfo
        {
            public string Alias { get; } = string.Empty;

            public string Description { get; } = string.Empty;

            public bool IsIsRequired { get; }

            public ArgumentInfo? Argument { get; set; } = null;
        }
    }
}
