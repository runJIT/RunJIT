using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.Generate.Client
{
    internal static class AddCurlBuilderExtension
    {
        internal static void AddCurlBuilder(this IServiceCollection services)
        {
            services.AddQueryBuilder();

            services.AddSingletonIfNotExists<CurlBuilder>();
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
    internal sealed class CurlBuilder()
    {
        private readonly string _curlBuilderTemplate = EmbeddedFile.GetFileContentFrom("RunJit.Generate.Client.Templates.CurlBuilder.rps");

        public string BuildFor(string projectName,
                               string clientName)
        {
            var clientFactory = _curlBuilderTemplate.Replace("$clientNameLower$", clientName.FirstCharToLower())
                                                    .Replace("$projectName$", projectName);

            return clientFactory;
        }
    }
}
