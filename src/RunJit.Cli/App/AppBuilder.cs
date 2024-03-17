using DotNetTool.Service;
using Extensions.Pack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli
{
    /// <summary>
    ///     App builder needed to setup dependency injection and create the cli (dotnet-tool) application.
    /// </summary>
    internal class AppBuilder
    {
        internal App Build()
        {
            // 1. Setup configuration
            var configurationBuilder = new ConfigurationBuilder();
            var appsettingsAsStream = typeof(AppBuilder).Assembly.GetEmbeddedFileAsStream("appsettings.json");
            var jsonStreamConfigurationSource = new JsonStreamConfigurationSource { Stream = appsettingsAsStream };

            configurationBuilder.Add(jsonStreamConfigurationSource);
            configurationBuilder.AddEnvironmentVariables();
            configurationBuilder.AddUserSecrets(typeof(AppBuilder).Assembly);
            var configuration = configurationBuilder.Build();

            // 2. Setup dependency injection
            var services = new ServiceCollection();
            var startup = new Startup();
            var dotnetTool = DotNetToolFactory.Create();

            services.AddSingletonIfNotExists<IDotNetTool>(dotnetTool);

            startup.ConfigureServices(services, configuration);

            var serviceProvider = services.BuildServiceProvider();

            return new App(serviceProvider);
        }
    }
}
