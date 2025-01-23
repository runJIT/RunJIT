using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.RunJit.Generate.Client;

namespace RunJit.Cli.Generate.Client
{
    internal static class AddClientFactoryBuilderExtension
    {
        internal static void AddClientFactoryBuilder(this IServiceCollection services)
        {
            services.AddUsingsBuilder();
            services.AddDependencyBuilder();

            services.AddSingletonIfNotExists<ClientFactoryBuilder>();
        }
    }

    /// <summary>
    ///     What we create here:
    ///     - We are just replace parameters from the RunJit.Generate.Client.Templates.client.factory.rps template to create
    ///     the
    ///     client factory
    /// </summary>
    internal sealed class ClientFactoryBuilder(UsingsBuilder usingsBuilder,
                                               DependencyBuilder dependencyBuilder)
    {
        private readonly string _clientFactoryTemplate = EmbeddedFile.GetFileContentFrom("RunJit.Generate.Client.Templates.client.factory.rps");

        public string BuildFor(string projectName,
                               string clientName,
                               GeneratedClient generatedClient)
        {
            var dependencies = dependencyBuilder.BuildFrom(generatedClient);
            var usings = usingsBuilder.BuildFrom(generatedClient, projectName);

            var clientFactory = _clientFactoryTemplate.Replace("$clientNameLower$", clientName.FirstCharToLower())
                                                      .Replace("$projectName$", projectName)
                                                      .Replace("$clientName$", clientName)
                                                      .Replace("$dependencies$", dependencies)
                                                      .Replace("$usings$", usings);

            return clientFactory;
        }
    }
}
