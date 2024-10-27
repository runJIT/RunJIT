using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Generate.Client
{
    internal static class AddHttpCallHandlerExtension
    {
        internal static void AddHttpCallHandler(this IServiceCollection services)
        {
            services.AddQueryBuilder();

            services.AddSingletonIfNotExists<HttpCallHandler>();
        }
    }

    // What we create here:
    // - we create here some new helper classes to create curl printer and more
    // 
    // Samples:
    //
    // Curl
    //   -> HttpCallHandler.cs
    //   -> RequestPrinter.cs
    internal sealed class HttpCallHandler()
    {
        private readonly string _httpCallHandlerTemplate = EmbeddedFile.GetFileContentFrom("RunJit.Generate.Client.Templates.HttpCallHandler.rps");

        public string BuildFor(string projectName,
                               string clientName)
        {
            var clientFactory = _httpCallHandlerTemplate.Replace("$clientNameLower$", clientName.FirstCharToLower())
                                                        .Replace("$projectName$", projectName);

            return clientFactory;
        }
    }
}
