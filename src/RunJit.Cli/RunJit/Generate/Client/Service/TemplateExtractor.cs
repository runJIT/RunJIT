using System.IO.Compression;
using System.Net.Http.Headers;
using AspNetCore.Simple.Sdk.Mediator;
using Extensions.Pack;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Api.Client;
using RunJit.Cli.Auth0;

namespace RunJit.Cli.RunJit.Generate.Client
{
    internal static class AddTemplateExtractorExtension
    {
        internal static void AddTemplateExtractor(this IServiceCollection services,
                                                  IConfiguration configuration)
        {
            services.AddSingletonIfNotExists<ITemplateExtractor, TemplateExtractor>();
            services.AddRunJitApiClientFactory(configuration);
            services.AddMediator();
        }
    }

    internal interface ITemplateExtractor
    {
        Task ExtractToAsync(DirectoryInfo directoryInfo,
                            ClientParameters clientGenParameters);
    }

    internal class TemplateExtractor(IRunJitApiClientFactory runJitApiClientFactory,
                                     IMediator mediator,
                                     IHttpClientFactory httpClientFactory,
                                     RunJitApiClientSettings runJitApiClientSettings) : ITemplateExtractor
    {
        public async Task ExtractToAsync(DirectoryInfo directoryInfo,
                                         ClientParameters clientGenParameters)
        {
            var auth = await mediator.SendAsync(new GetTokenByStorageCache()).ConfigureAwait(false);
            var httpClient = httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(runJitApiClientSettings.BaseAddress);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(auth.TokenType, auth.Token);
            var rRunJitApiClient = runJitApiClientFactory.CreateFrom(httpClient);

            var generateClientResponse = await rRunJitApiClient.Clients.V1.GenerateClientAsync().ConfigureAwait(false);

            using var zipArchive = new ZipArchive(generateClientResponse.FileStream);
            zipArchive.ExtractToDirectory(directoryInfo.FullName, true);
        }
    }
}
