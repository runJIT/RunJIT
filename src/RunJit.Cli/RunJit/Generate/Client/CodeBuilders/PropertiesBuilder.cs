using System.Collections.Immutable;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Generate.Client
{
    internal static class AddPropertiesBuilderExtension
    {
        internal static void AddPropertiesBuilder(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<PropertiesBuilder>();
        }
    }

    // What we are create here:
    // - We create the properties declarations
    //
    // Sample:
    // 
    // public AdminV1 V1 { get; init; }
    internal class PropertiesBuilder
    {
        private readonly string _propertyTemplate = EmbeddedFile.GetFileContentFrom("RunJit.Generate.Client.Templates.property.rps");

        internal string BuildFrom(IImmutableList<GeneratedFacade> facades)
        {
            var parameters = facades.Select(f => _propertyTemplate.Replace("$name$", $"{f.FacadeName}")
                                                                  .Replace("$version$", f.Domain))
                                    .Flatten(Environment.NewLine);

            return parameters;
        }

        internal string BuildFrom(IGrouping<string, GeneratedClientCodeForController> groupedEndpoints)
        {
            var properties = groupedEndpoints.Select(f => _propertyTemplate.Replace("$name$", f.Domain)
                                                                           .Replace("$version$", f.ControllerInfo.Version.Normalized)
                                                                           .Replace("$attributes$", f.ControllerInfo.ObsoleteInfo.IsNull() ? string.Empty : $"""[Obsolete("{f.ControllerInfo.ObsoleteInfo.Info}")]"""))
                                             .Flatten(Environment.NewLine);

            return properties;
        }
    }
}
