using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.RunJit.Generate.Client;
using Solution.Parser.CSharp;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal static class AddModelBuilderExtension
    {
        internal static void AddModelBuilder(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<ModelBuilder>();
        }
    }

    // What we are create here:
    // - We create classes which contains the copied data class from the api
    // - We only replace in the template Pulse.Generate.DotNetTool.Templates.model.rps the place holders with the expected values
    // Sample:
    //
    // using System;
    // using Pulse.Common.Data.V3;

    // namespace PulseCore.DotNetTool.Api.BulkResources.V3
    // {
    //     public record ResourceToResourceConfigurationItem
    //     {
    //         public string ParentRef { get; set; } = string.Empty;
    //         public long ParentId { get; set; }     
    //         public IImmutableDictionary<string, string> Properties { get; init; } = ImmutableDictionary<string, string>.Empty;
    //     }
    // }
    public class ModelBuilder
    {
        private readonly string _modelTemplate = EmbeddedFile.GetFileContentFrom("Pulse.Generate.DotNetTool.Templates.model.rps");

        public string BuildFrom(DeclarationBase dataType,
                                GeneratedDotNetToolCodeForController controller,
                                string projectName,
                                string clientName)
        {
            var @namespace = $"{projectName}.{ClientGenConstants.Api}.{controller.ControllerInfo.DomainName}.{controller.ControllerInfo.Version.Normalized}";
            var model = _modelTemplate.Replace("$projectName$", projectName)
                                      .Replace("$clientName$", clientName)
                                      .Replace("$namespace$", @namespace)
                                      .Replace("$syntaxTree$", dataType.SyntaxTree);

            return model;
        }
    }
}
