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
app.UseSimpleMinimalApiEnvironment("api/core", endpoint =>
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
