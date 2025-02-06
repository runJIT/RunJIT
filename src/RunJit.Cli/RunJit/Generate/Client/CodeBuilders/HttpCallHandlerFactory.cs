﻿using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.Generate.Client
{
    internal static class AddHttpCallHandlerFactoryExtension
    {
        internal static void AddHttpCallHandlerFactory(this IServiceCollection services)
        {
            services.AddQueryBuilder();

            services.AddSingletonIfNotExists<HttpCallHandlerFactory>();
        }
    }

    // What we create here:
    // - we create here some new helper classes to create curl printer and more
    // 
    // Samples:
    //
    // Curl
    //   -> HttpCallHandlerFactory.cs
    //   -> RequestPrinter.cs
    internal sealed class HttpCallHandlerFactory
    {
        private readonly string _httpCallHandlerFactoryTemplate = EmbeddedFile.GetFileContentFrom("RunJit.Generate.Client.Templates.HttpCallHandlerFactory.rps");

        public string BuildFor(string projectName,
                               string clientName)
        {
            var clientFactory = _httpCallHandlerFactoryTemplate.Replace("$clientNameLower$", clientName.FirstCharToLower())
                                                               .Replace("$projectName$", projectName);

            return clientFactory;
        }
    }
}
