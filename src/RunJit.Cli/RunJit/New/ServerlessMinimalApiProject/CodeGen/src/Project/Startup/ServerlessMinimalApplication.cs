using Amazon.Lambda.Serialization.SystemTextJson;
using Asp.Versioning;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Siemens.AspNet.ErrorHandling;
using $ProjectName$.IdentityProvider;
using $ProjectName$.JsonSerializing;
using $ProjectName$.OpenApi;

namespace $ProjectName$
{
    public partial class Program;
}

namespace $ProjectName$.Startup
{
    public class ServerlessMinimalWebApi
    {
        public Action<IServiceCollection, IConfiguration> RegisterServices { get; set; } = (services, configuration) => { };

        public Action<WebApplication> SetupApplication { get; set; } = app => { };

        public Action<IEndpointRouteBuilder> MapEndpoints { get; set; } = app => { };

        public void Run(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // OpenTelemetry and Logging
            builder.Services.AddErrorHandling();

            // AWS Lambda Hosting
            builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi, new SourceGeneratorLambdaJsonSerializer<AppJsonSerializerContext>());

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddJsonSerializeOptions();

            // Add API versioning
            // We need a version locator :) 
            // To iterate over all versions
            builder.Services.AddApiVersioning(apiVersion =>
                                              {
                                                  apiVersion.DefaultApiVersion = new ApiVersion(1, 0);
                                                  apiVersion.ApiVersionReader = new UrlSegmentApiVersionReader();
                                              }).AddApiExplorer(apiExplorer =>
                                                                {
                                                                    apiExplorer.GroupNameFormat = "'v'V";
                                                                    apiExplorer.SubstituteApiVersionInUrl = true;
                                                                });



            // Open API
            // ToDo: We need version detection for minimal apis
            // Old api version provider works only for Controller based APIs
            builder.Services.AddOpenApiInfos(builder.Configuration);

            var versionNumber = new int[] { 1, 2 };
            foreach (var version in versionNumber)
            {
                builder.Services.AddOpenApi($"v{version}", options =>
                                                           {
                                                               options.AddDocumentTransformer<DocumentInfosTransformer>();
                                                               options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
                                                           });
            }

            // Add Identity Provider
            builder.Services.AddCognito(builder.Configuration);

            // Add Authentication and Authorization
            builder.Services.AddAuthorization();

            // Health checks
            builder.Services.AddHealthChecks();

            // Custom registrations
            RegisterServices(builder.Services, builder.Configuration);

            // Build Application
            var app = builder.Build();

            // Middleware Pipeline

            // Error Handling Middleware
            app.UseErrorHandling();

            app.UsePathBase($"/api/core");

            // Special we want to have version independent health check
            // 1. Set up the version based health
            app.MapHealthChecks("health", new HealthCheckOptions()
                                          {
                                              ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                                          });

            // Map open api
            app.MapOpenApi();

            // Setup API versioning
            // ToDo: We need a version collector which collects all versions
            var apiVersionSet = app.NewApiVersionSet()
                                   .HasApiVersion(new ApiVersion(1))
                                   // .HasApiVersion(new ApiVersion(2))
                                   .ReportApiVersions()
                                   .Build();

            // Setup base path for API versioning
            var basePath = app.MapGroup("/v{apiVersion:apiVersion}").WithApiVersionSet(apiVersionSet);

            // Endpoints must be registered before use routing is used
            MapEndpoints(basePath);

            app.UseRouting();

            // Authentication & Authorization Middleware
            app.UseAuthentication();
            app.UseAuthorization();


            SetupApplication(app);

            app.Run();
        }
    }
}
