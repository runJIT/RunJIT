using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.Generate.Client
{
    internal static class AddRequestPrinterExtension
    {
        internal static void AddRequestPrinter(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<RequestPrinter>();
        }
    }

    // What we create here:
    // - we create here some new helper classes to create curl printer and more
    // 
    // Samples:
    //
    // Curl
    //   -> CurlBuilder.cs
    //   -> RequestPrinter.cs
    internal sealed class RequestPrinter
    {
        private readonly string _curlBuilderTemplate = EmbeddedFile.GetFileContentFrom("RunJit.Generate.Client.Templates.RequestPrinter.rps");

        public string BuildFor(string projectName,
                               string clientName)
        {
            var clientFactory = _curlBuilderTemplate.Replace("$clientNameLower$", clientName.FirstCharToLower())
                                                    .Replace("$projectName$", projectName)
                                                    .Replace("$clientName$", clientName);

            return clientFactory;
        }
    }
}
