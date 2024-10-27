using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.RunJit.Generate.Client;
using RunJit.Cli.Services.Endpoints;
using Solution.Parser.CSharp;
using Attribute = Solution.Parser.CSharp.Attribute;

namespace RunJit.Cli.Services
{
    internal static class AddMinimalApiEndpointParserExtension
    {
        internal static void AddMinimalApiEndpointParser(this IServiceCollection services)
        {
            services.AddMethodParser();

            services.AddSingletonIfNotExists<MinimalApiEndpointParser>();
        }
    }

    //public static class GetAllToDoEndpoints
    //{
    //    public static RouteHandlerBuilder MapGetAll(this IEndpointRouteBuilder routeGroupBuilder)
    //    {
    //        return routeGroupBuilder.MapPut("todos", ([FromServices] GetAllToDosQuery getAllToDosQuery) =>
    //                                {
    //                                    return Results.Ok(getAllToDosQuery.Execute());
    //                                }).WithSummary("Get all ToDos")
    //                                .Produces(200, typeof(Todo))
    //                                .Produces(400, typeof(ProblemDetails))
    //                                .Produces(500, typeof(ProblemDetails))
    //                                // .WithName("getAllToDos")
    //                                .WithSummary("Returns all available ToDos")
    //                                .WithTags("ToDo")
    //                                .WithDescriptionFromFile("V1.GetAll.Documentation.GetAllTodoDescription.txt")
    //                                .WithDescription("Legacy version")
    //                                .WithOpenApi()
    //                                .MapToApiVersion(1);
    //    }
    //}

    //[DebuggerDisplay("{Name}")]
    //public record EndpointInfo
    //{
    //    public required string Name { get; init; } --> MapGetAll // GetAll

    //    public required string DomainName { get; init; } --> .WithTags("ToDo")

    //    public required VersionInfo Version { get; init; }  --> .MapToApiVersion(1);

    //    public required string BaseUrl { get; init; } --> var basePath = webApplication.MapGroup("api/v{apiVersion:apiVersion}").WithApiVersionSet(apiVersionSet);

    //    public required string GroupName { get; init; }   --> .WithTags("ToDo")

    //    public required string SwaggerOperationId { get; init; } --> .WithName("getAllToDos") -> Problem here must be unique over all version which is bad :(

    //    public required string HttpAction { get; init; } --> .MapPut

    //    public required string RelativeUrl { get; init; }  --> MapPut("todos",

    //    public required IImmutableList<Parameter> Parameters { get; init; } = ImmutableList<Parameter>.Empty;

    //    public required RequestType? RequestType { get; init; } --> parameter body which is not a native .Net type :)

    //    public required ResponseType ResponseType { get; init; } --> return Results.Ok(getAllToDosQuery.Execute()); 

    //    public required IImmutableList<ProduceResponseTypes> ProduceResponseTypes { get; init; }  --> .Produces(400, typeof(ProblemDetails))

    //    public ImmutableList<DeclarationBase> Models { get; init; } = ImmutableList<DeclarationBase>.Empty;
    //}

    internal sealed class MinimalApiEndpointParser(DataTypeFinder dataTypeFinder)
    {
        private string[] mapActions = new string[]
                                      {
                                          ".MapGet(", ".MapPost(", ".MapDelete(",
                                          ".MapPut(", ".MapPatch("
                                      };

        internal IImmutableList<EndpointInfo> ExtractFrom(IImmutableList<CSharpSyntaxTree> syntaxTrees,
                                                          IImmutableList<Type> reflectionTypes)
        {
            var basePath = FindBasePath(syntaxTrees);

            // 1. Detect all controllers. Derived class like Controller, ControllerBase, ODataController and so on
            var endpointMappings = GetAllStatements(basePath, syntaxTrees, reflectionTypes);

            return endpointMappings;

            //// 2. Extract all needed infos for generation out
            //var controllerInfos = Parse(endpointMappings, reflectionTypes, syntaxTrees).ToImmutableList();

            //return controllerInfos;
        }

        private string FindBasePath(IImmutableList<CSharpSyntaxTree> syntaxTrees)
        {
            foreach (var cSharpSyntaxTree in syntaxTrees)
            {
                foreach (var statement in cSharpSyntaxTree.Statements)
                {
                    if (statement.SyntaxTree.Contains("MapGroup") && statement.SyntaxTree.Contains(".WithApiVersionSet"))
                    {
                        var result = Regex.Match(statement.SyntaxTree, @"MapGroup\(\""(.*?)\""");

                        if (result.Success)
                        {
                            return result.Groups[1].Value;
                        }
                    }
                }

                foreach (var @class in cSharpSyntaxTree.Classes)
                {
                    foreach (var method in @class.Methods)
                    {
                        foreach (var statement in method.Statements)
                        {
                            if (statement.Contains("MapGroup") && statement.Contains(".WithApiVersionSet"))
                            {
                                var result = Regex.Match(statement, @"MapGroup\(\""(.*?)\""");

                                if (result.Success)
                                {
                                    return result.Groups[1].Value;
                                }
                            }
                        }
                    }
                }
            }

            return string.Empty;
        }

        private IImmutableList<EndpointInfo> GetAllStatements(string basePath,
                                                              IImmutableList<CSharpSyntaxTree> syntaxTrees,
                                                              IImmutableList<Type> reflectionTypes)
        {
            var listStatements = ImmutableList<EndpointInfo>.Empty;

            foreach (var syntaxTree in syntaxTrees)
            {
                foreach (var @class in syntaxTree.Classes)
                {
                    foreach (var method in @class.Methods)
                    {
                        foreach (var methodStatement in method.Statements)
                        {
                            foreach (var mapAction in mapActions)
                            {
                                if (methodStatement.Contains(mapAction))
                                {
                                    var version = ExtractVersion(methodStatement);
                                    var produceResponseTypes = ExtractProduceResponseTypes(methodStatement);
                                    var payloads = ExtractPayload(methodStatement).ToList();
                                    var normalizedBasePath = basePath.Replace("{apiVersion:apiVersion}", version.Original);
                                    var relativeUrl = ExtractRelativeUrl(methodStatement);
                                    var url = $"{normalizedBasePath}/{relativeUrl}";
                                    var allUsedModels = GetAllUsedModels(produceResponseTypes, syntaxTrees, reflectionTypes, version, payloads);

                                    var endpointInfo = new EndpointInfo
                                                       {
                                                           Name = ExtractName(methodStatement),
                                                           DomainName = ExtractDomainName(methodStatement),
                                                           Version = version,
                                                           BaseUrl = normalizedBasePath,
                                                           GroupName = ExtractGroupName(methodStatement),
                                                           SwaggerOperationId = ExtractSwaggerOperationId(methodStatement, version),
                                                           HttpAction = ExtractHttpAction(methodStatement),
                                                           RelativeUrl = url,
                                                           Parameters = ExtractParameters(methodStatement),
                                                           RequestType = ExtractRequestType(payloads, allUsedModels, reflectionTypes),
                                                           ResponseType = ExtractResponseType(produceResponseTypes),
                                                           ProduceResponseTypes = produceResponseTypes,
                                                           Models = allUsedModels
                                                       };

                                    listStatements = listStatements.Add(endpointInfo);
                                }
                            }
                        }
                    }
                }
            }

            return listStatements;
        }

        private IImmutableList<DeclarationBase> GetAllUsedModels(IImmutableList<ProduceResponseTypes> produceResponseTypes,
                                                                 IImmutableList<CSharpSyntaxTree> syntaxTrees,
                                                                 IImmutableList<Type> reflectionTypes,
                                                                 VersionInfo versionInfo,
                                                                 IEnumerable<string> payloads)
        {
            var responseType = produceResponseTypes.FirstOrDefault(p => p.StatusCode >= 200 && p.StatusCode < 300)?.Type;
            
            var match = GlobalRegex.GetGenericTypeRegex().Match(responseType ?? string.Empty);

            if (match.Success)
            {
                responseType = match.Groups[0].Value.TrimStart('<').TrimEnd('>');
            }

            var allModelsToFind = payloads.Concat(responseType);
            
            var classes = syntaxTrees.SelectMany(tree => tree.Classes).Where(c => allModelsToFind.Any(x => x == c.Name) && c.FullQualifiedName.Contains(versionInfo.Normalized)).ToList();
            var records = syntaxTrees.SelectMany(tree => tree.Records).Where(c => allModelsToFind.Any(x => x == c.Name) && c.FullQualifiedName.Contains(versionInfo.Normalized)).ToList();

            var allTypes = classes.Concat(records).OfType<DeclarationBase>().ToImmutableList();

            var typeNames = allTypes.Select(t => t.FullQualifiedName).ToImmutableList();
            var types = dataTypeFinder.FindDataType(typeNames, syntaxTrees, reflectionTypes);

            var result = types.Select(t => t.Declaration).ToImmutableList();

            return result;
        }

        private ResponseType ExtractResponseType(IImmutableList<ProduceResponseTypes> produceResponseTypes)
        {
            var responseType = produceResponseTypes.FirstOrDefault(p => p.StatusCode >= 200 && p.StatusCode < 300)?.Type;

            if (responseType.IsNull())
            {
                return new ResponseType("void", "void");
            }

            return new ResponseType(responseType, responseType);
        }

        private string ExtractName(string code)
        {
            var match = Regex.Match(code, @"\.WithTags\(""(?<tag>[^""]+)""\)");

            return match.Success ? match.Groups["tag"].Value : string.Empty;
        }

        private string ExtractDomainName(string code)
        {
            var match = Regex.Match(code, @"\.WithTags\(""(?<tag>[^""]+)""\)");

            return match.Success ? match.Groups["tag"].Value : string.Empty;
        }

        private VersionInfo ExtractVersion(string code)
        {
            var match = Regex.Match(code, @"\.MapToApiVersion\((?<version>\d+)\)");
            var majorVersion = match.Groups["version"].Value.ToIntOrDefault();

            return new VersionInfo($"{majorVersion}.0", $"V{majorVersion}");
        }

        private string ExtractBaseUrl(string version)
        {
            return $"api/{version}";
        }

        private string ExtractGroupName(string code)
        {
            var match = Regex.Match(code, @"\.WithTags\(""(?<tag>[^""]+)""\)");

            return match.Success ? match.Groups["tag"].Value : string.Empty;
        }

        private string ExtractSwaggerOperationId(string code,
                                                 VersionInfo versionInfo)
        {
            var match = Regex.Match(code, @"\.WithName\(""(?<name>[^""]+)""\)");

            return match.Success ? match.Groups["name"].Value.Replace(versionInfo.Normalized, string.Empty) : string.Empty;
        }

        private string ExtractHttpAction(string code)
        {
            var match = Regex.Match(code, @"\.Map(?<action>\w+)\(");

            return match.Success ? match.Groups["action"].Value : string.Empty;
        }

        private string ExtractRelativeUrl(string code)
        {
            var match = Regex.Match(code, @"Map\w+\(\""(.*?)\""");

            return match.Success ? match.Groups[1].Value : string.Empty;
        }

        private IImmutableList<Parameter> ExtractParameters(string code)
        {
            var regex = new Regex(@"\((.*?)\)\s*=>");
            var regexBrackets = new Regex(@"\((.*?)\)");

            var match = regex.Match(code);

            if (match.Success)
            {
                var method = match.Value.Replace(" =>", string.Empty);

                var parameters = method.TrimStart('(').TrimEnd(')').Split(",", StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim().TrimStart('(').TrimEnd(')'));
                var ignoreServices = parameters.Where(p => p.DoesNotContain("[FromService") && p.DoesNotContain("Http")).Where(p => p.Contains(" ")).ToList();
                var result = Parse(ignoreServices).ToImmutableList();

                return result;
            }

            return ImmutableList<Parameter>.Empty;

            IEnumerable<Parameter> Parse(IEnumerable<string> parameters)
            {
                foreach (var parameter in parameters)
                {
                    // int i
                    // int i = 0
                    // [FromQuery] string search
                    // [FromServices] IService service
                    var isOptional = parameter.Contains("=");

                    var splitted = parameter.Split(" = ").First().Split(" ");
                    var defaultValue = isOptional ? parameter.Split(" = ").Last() : null;

                    var attribute = parameter.StartsWith('[')
                                        ? ImmutableList.Create<Attribute>(new Attribute(splitted[0].TrimStart('[').TrimEnd(']'), ImmutableList<string>.Empty, parameter,
                                                                                        string.Empty))
                                        : ImmutableList<Attribute>.Empty;

                    var type = splitted.Length == 3 ? splitted[1] : splitted[0];
                    var name = splitted.Length == 3 ? splitted[2] : splitted[1];

                    yield return new Parameter(type, name, attribute,
                                               parameter, isOptional, defaultValue,
                                               string.Empty);
                }
            }
        }

        private RequestType? ExtractRequestType(List<string> payloads,
                                                IImmutableList<DeclarationBase> models,
                                                IImmutableList<Type> reflectionTypes)
        {
            // Implement logic to extract request type
            var match = models.FirstOrDefault(m => payloads.Contains(m.Name));

            if (match.IsNull())
            {
                return null;
            }

            var reflectionType = reflectionTypes.First(item => item.FullName == match.FullQualifiedName);

            return new RequestType(match, reflectionType);
        }

        //private ResponseType ExtractResponseType(string code,
        //                                         IImmutableList<CSharpSyntaxTree> syntaxTrees,
        //                                         IImmutableList<Type> reflectionTypes)
        //{
        //    //return routeGroupBuilder.MapGet("todos", ([FromServices] GetAllToDosQuery getAllToDosQuery) =>
        //    //                        {
        //    //                            return Results.Ok(getAllToDosQuery.Execute());
        //    //                        }).WithSummary("Get all ToDos")

        //    // We have to find out the return type which is hard here

        //    // 1. find the return Results.Ok

        //    // Implement logic to extract response type
        //    return new ResponseType("", "");
        //}

        private IImmutableList<ProduceResponseTypes> ExtractProduceResponseTypes(string code)
        {
            var produceResponseTypes = ImmutableList.CreateBuilder<ProduceResponseTypes>();

            var matches = Regex.Matches(code, @"\.Produces\((?<code>\d+), typeof\((?<type>[^\)]+)\)\)");

            foreach (Match match in matches)
            {
                var produceResponsetype = new ProduceResponseTypes(match.Groups["type"].Value, match.Groups["code"].Value.ToIntOrDefault());
                produceResponseTypes.Add(produceResponsetype);
            }

            return produceResponseTypes.ToImmutable();
        }
        
        private IEnumerable<string> ExtractPayload(string code)
        {
            var matches = Regex.Matches(code, @"\[FromBody\]\s*([^\s]+)");

            foreach (Match match in matches)
            {
                yield return match.Groups[1].Value;
            }
        }
    }
}
