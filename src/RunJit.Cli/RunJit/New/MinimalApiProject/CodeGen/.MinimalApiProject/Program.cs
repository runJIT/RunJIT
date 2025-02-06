using System.Xml.Linq;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;
using Solution.Parser.CSharp;

namespace RunJit.Cli.New.MinimalApiProject.CodeGen.MinimalApiProject
{
    internal static class AddProgramCodeGenExtension
    {
        internal static void AddProgramCodeGen(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<IMinimalApiProjectSpecificCodeGen, ProgramCodeGen>();
        }
    }

    internal sealed class ProgramCodeGen(ConsoleService consoleService) : IMinimalApiProjectSpecificCodeGen
    {
        private const string Template = """
                                        using System.Text.Json.Serialization;
                                        using Amazon.Lambda.APIGatewayEvents;
                                        using Amazon.Lambda.Serialization.SystemTextJson;
                                        using Siemens.AspNet.ErrorHandling;
                                        using Siemens.Data.Cloud.Core.ExtensionMethods;
                                        using Siemens.Data.Cloud.Core.Shared.Authentication;
                                        using Siemens.Data.Cloud.Core.Shared.OpenTelemetry;
                                        
                                        var builder = WebApplication.CreateBuilder(args);
                                        
                                        // OpenTelemetry and Logging
                                        builder.AddOpenTelemetryXRay();
                                        builder.Services.AddErrorHandling();
                                        
                                        // AWS Lambda Hosting
                                        builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi, new SourceGeneratorLambdaJsonSerializer<AppJsonSerializerContext>());
                                        
                                        // Service Registrations
                                        builder.Services.AddSimpleMinimalApiEnvironment(builder.Configuration);
                                        
                                        // Domain registrations
                                        builder.Services.AddApi(builder.Configuration);
                                        
                                        // Add Identity Provider
                                        builder.Services.AddCognito(builder.Configuration);
                                        
                                        // Add Authentication and Authorization
                                        builder.Services.AddAuthorization();
                                        
                                        // Build Application
                                        var app = builder.Build();
                                        
                                        // Middleware Pipeline
                                        
                                        // Error Handling Middleware
                                        app.UseErrorHandling();
                                        
                                        // Use optimized startup and setups
                                        app.UseSimpleMinimalApiEnvironment("$basePath$", endpoint =>
                                        {
                                            endpoint.UseApiDomains();
                                        });
                                        
                                        // Authentication & Authorization Middleware
                                        app.UseAuthentication();
                                        app.UseAuthorization();
                                        
                                        // Run Application
                                        app.Run();
                                        
                                        // We need this declaration to use this program as entry
                                        // point for the microsoft test host as entry point
                                        public partial class Program;
                                        """;

        public async Task GenerateAsync(FileInfo projectFileInfo,
                                        XDocument projectDocument,
                                        MinimalApiProjectInfos minimalApiProjectInfos)
        {
            // 1. Add AppBuilder.cs
            var file = Path.Combine(projectFileInfo.Directory!.FullName, "Program.cs");

            var newTemplate = Template.Replace("$namespace$", minimalApiProjectInfos.ProjectName)
                                      .Replace("$dotNetToolName$", minimalApiProjectInfos.NormalizedName)
                                      .Replace("$basePath$", minimalApiProjectInfos.BasePath);

            var formattedTemplate = newTemplate.FormatSyntaxTree();

            await File.WriteAllTextAsync(file, formattedTemplate).ConfigureAwait(false);

            // 2. Print success message
            consoleService.WriteSuccess($"Successfully created {file}");
        }
    }
}
