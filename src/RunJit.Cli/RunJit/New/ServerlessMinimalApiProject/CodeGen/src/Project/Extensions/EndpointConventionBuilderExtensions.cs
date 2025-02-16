using Extensions.Pack;

namespace $ProjectName$.Extensions
{
    internal static class EndpointConventionBuilderExtensions
    {
        internal static TBuilder WithSummaryFromFile<TBuilder>(this TBuilder builder,
                                                               string embeddedFile)
            where TBuilder : IEndpointConventionBuilder
        {
            var fileContent = EmbeddedFile.GetFileContentFrom(embeddedFile);

            return builder.WithSummary(fileContent);
        }

        internal static TBuilder WithDescriptionFromFile<TBuilder>(this TBuilder builder,
                                                                   string embeddedFile)
            where TBuilder : IEndpointConventionBuilder
        {
            var fileContent = EmbeddedFile.GetFileContentFrom(embeddedFile);

            return builder.WithDescription(fileContent);
        }
    }
}
