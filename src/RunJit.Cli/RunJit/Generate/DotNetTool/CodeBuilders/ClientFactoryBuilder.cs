using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal static class AddDotNetToolFactoryBuilderExtension
    {
        internal static void AddDotNetToolFactoryBuilder(this IServiceCollection services)
        {
            services.AddUsingsBuilder();
            services.AddDependencyBuilder();

            services.AddSingletonIfNotExists<DotNetToolFactoryBuilder>();
        }
    }

    /// <summary>
    ///     What we create here:
    ///     - We are just replace parameters from the Pulse.Generate.DotNetTool.Templates.client.factory.rps template to create the
    ///     client factory
    /// </summary>
    internal class DotNetToolFactoryBuilder(
        UsingsBuilder usingsBuilder,
        DependencyBuilder dependencyBuilder)
    {
        private readonly string _clientFactoryTemplate = EmbeddedFile.GetFileContentFrom("Pulse.Generate.DotNetTool.Templates.client.factory.rps");

        public string BuildFor(string projectName, string clientName, GeneratedDotNetTool generatedDotNetTool)
        {
            var dependencies = dependencyBuilder.BuildFrom(generatedDotNetTool);
            var usings = usingsBuilder.BuildFrom(generatedDotNetTool, projectName);

            var clientFactory = _clientFactoryTemplate.Replace("$projectName$", projectName)
                                                      .Replace("$clientName$", clientName)
                                                      .Replace("$dependencies$", dependencies)
                                                      .Replace("$usings$", usings);
            return clientFactory;
        }
    }
}
