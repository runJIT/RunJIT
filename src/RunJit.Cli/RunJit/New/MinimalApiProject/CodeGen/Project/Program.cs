using $ProjectName$.Api;
using $ProjectName$.Startup;

var webApi = new ServerlessMinimalWebApi();

webApi.RegisterServices = (service, config) =>
{
    // Domain service registrations
    service.AddApi(config);
};

webApi.MapEndpoints = endpoints =>
{
    // Map api domain endpoints
    endpoints.MapApi();
};

webApi.Run(args);
