using Extensions.Pack;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace $ProjectName$.OpenApi
{
    internal sealed class DocumentInfosTransformer(OpenApiInfos openApiInfos) : IOpenApiDocumentTransformer
    {
        public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
        {
            var documentSpecificInfos = openApiInfos.Versions.FirstOrDefault(doc => doc.Version.Contains(context.DocumentName.TrimStart('v').TrimStart('V')));

            if (documentSpecificInfos.IsNotNull())
            {
                document.Info = new()
                {
                    Title = documentSpecificInfos.Title,
                    Version = documentSpecificInfos.Version,
                    Description = documentSpecificInfos.Description,
                    Contact = new OpenApiContact()
                    {
                        Email = documentSpecificInfos.ContactEmail,
                        Name = documentSpecificInfos.ContactName,
                    }
                };
            }
            else
            {
                document.Info = new()
                {
                    Title = "Your first Open API",
                    Version = "v1",
                    Description = "API for Damien"
                };
            }

            return Task.CompletedTask;
        }
    }
}
