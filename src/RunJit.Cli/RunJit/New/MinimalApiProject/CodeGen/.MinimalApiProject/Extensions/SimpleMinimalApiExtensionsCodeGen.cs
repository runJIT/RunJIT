using System.Xml.Linq;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Generate.DotNetTool;
using RunJit.Cli.Services;
using Solution.Parser.CSharp;

namespace RunJit.Cli.New.MinimalApiProject.CodeGen.MinimalApiProject.Extensions
{
    internal static class AddSimpleMinimalApiExtensionsCodeGenExtension
    {
        internal static void AddSimpleMinimalApiExtensionsCodeGen(this IServiceCollection services)
        {
            services.AddConsoleService();

            services.AddSingletonIfNotExists<IDotNetToolSpecificCodeGen, SimpleMinimalApiExtensionsCodeGen>();
        }
    }

    internal sealed class SimpleMinimalApiExtensionsCodeGen(ConsoleService consoleService,
                                                            NamespaceProvider namespaceProvider) : IDotNetToolSpecificCodeGen
    {
        private const string Template = """
                                        using System.Text.Json;
                                        using System.Text.Json.Serialization;
                                        using Asp.Versioning;
                                        using Extensions.Pack;
                                        
                                        namespace $namespace$
                                        {
                                            internal static class SimpleMinimalApiExtensions
                                            {
                                                internal static void AddSimpleMinimalApiEnvironment(this IServiceCollection services,
                                                                                                  IConfiguration _)
                                                {
                                                    services.AddEndpointsApiExplorer();
                                        
                                                    // Add API versioning
                                                    services.AddApiVersioning(apiVersion =>
                                                                              {
                                                                                  apiVersion.DefaultApiVersion = new ApiVersion(1, 0);
                                                                                  apiVersion.ApiVersionReader = new UrlSegmentApiVersionReader();
                                                                              }).AddApiExplorer(apiExplorer =>
                                                                                                {
                                                                                                    apiExplorer.GroupNameFormat = "'v'V";
                                                                                                    apiExplorer.SubstituteApiVersionInUrl = true;
                                                                                                });
                                        
                                                    // services.AddOpenApi("v1");
                                                    // services.AddOpenApi("v2");
                                                    services.ConfigureHttpJsonOptions(options =>
                                                                                      {
                                                                                          options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
                                                                                          options.SerializerOptions.PropertyNameCaseInsensitive = true;
                                                                                          options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                                                                                          options.SerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString;
                                                                                          options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
                                                                                      });
                                                }
                                        
                                                internal static void UseSimpleMinimalApiEnvironment(this WebApplication webApplication,
                                                                                                  string basePathString,
                                                                                                  Action<IEndpointRouteBuilder> endpointRegistration)
                                                {
                                                    webApplication.UsePathBase($"/{basePathString}");
                                        
                                                    // Setup API versioning
                                                    var apiVersionSet = webApplication.NewApiVersionSet()
                                                                                      .HasApiVersion(new ApiVersion(1))
                                        
                                                                                      // .HasApiVersion(new ApiVersion(2))
                                                                                      .ReportApiVersions()
                                                                                      .Build();
                                        
                                                    // Setup base path for API versioning
                                                    var basePath = webApplication.MapGroup("/v{apiVersion:apiVersion}").WithApiVersionSet(apiVersionSet);
                                        
                                                    endpointRegistration(basePath);
                                                    webApplication.UseRouting();
                                        
                                                    // webApplication.MapOpenApi();
                                                }
                                            }
                                        
                                            internal static class EndpointConventionBuilderExtensions
                                            {
                                                internal static TBuilder WithSummaryFromFile<TBuilder>(this TBuilder builder,
                                                                                                     string embeddedFile)
                                                    where TBuilder : IEndpointConventionBuilder
                                                {
                                                    var fileContent = EmbeddedFile.GetFileContentFrom(embeddedFile);
                                        
                                                    return builder.WithSummary(fileContent);
                                                }
                                        
                                                internal static TBuilder WithDescriptionFromFile<TBuilder>(this TBuilder builder,
                                                                                                         string embeddedFile)
                                                    where TBuilder : IEndpointConventionBuilder
                                                {
                                                    var fileContent = EmbeddedFile.GetFileContentFrom(embeddedFile);
                                        
                                                    return builder.WithDescription(fileContent);
                                                }
                                            }
                                        }
                                        
                                        """;

        public async Task GenerateAsync(FileInfo projectFileInfo,
                                        XDocument projectDocument,
                                        DotNetToolInfos dotNetToolInfos)
        {
            // 1. Add AppSettings.cs
            var file = Path.Combine(projectFileInfo.Directory!.FullName, "Extensions", "SimpleMinimalApiExtensions.json");

            var newTemplate = Template.Replace("$namespace$", dotNetToolInfos.ProjectName)
                                      .Replace("$dotNetToolName$", dotNetToolInfos.NormalizedName);

            var formattedTemplate = newTemplate.FormatSyntaxTree();

            await File.WriteAllTextAsync(file, formattedTemplate).ConfigureAwait(false);

            namespaceProvider.SetNamespaceProviderAsync(projectFileInfo, "Extensions", false);

            // 4. Print success message
            consoleService.WriteSuccess($"Successfully created {file}");
        }
    }
}
