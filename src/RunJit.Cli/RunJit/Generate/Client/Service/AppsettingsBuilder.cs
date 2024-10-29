﻿using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Generate.Client
{
    internal static class AddAppsettingsBuilderExtension
    {
        internal static void AddAppsettingsBuilder(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<AppsettingsBuilder>();
        }
    }

    internal sealed class AppsettingsBuilder
    {
        private readonly string _clientTemplate = EmbeddedFile.GetFileContentFrom("RunJit.Generate.Client.Templates.appsettings.test.json");

        public string BuildFor(string projectName,
                               string clientName)
        {
            var appsettings = _clientTemplate.Replace("$moduleName$", clientName);

            return appsettings;
        }
    }
}
