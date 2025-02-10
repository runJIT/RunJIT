using Extensions.Pack;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;

namespace $ProjectName$.OpenApi
{
    internal sealed class BearerSecuritySchemeTransformer(IAuthenticationSchemeProvider authenticationSchemeProvider,
                                                          OpenApiInfos openApiInfos) : IOpenApiDocumentTransformer
    {
        public async Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
        {
            var authenticationSchemes = await authenticationSchemeProvider.GetAllSchemesAsync().ConfigureAwait(false);
            if (authenticationSchemes.Any(authScheme => authScheme.Name == "Bearer"))
            {
                var requirements = new Dictionary<string, OpenApiSecurityScheme>
                {
                    ["Bearer"] = new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer", // "bearer" refers to the header name here, must be lowercase
                        In = ParameterLocation.Header,
                        BearerFormat = "Json Web Token"
                    }
                };
                document.Components ??= new OpenApiComponents();
                document.Components.SecuritySchemes = requirements;
            }

            var version = openApiInfos.Versions.FirstOrDefault(doc => doc.Version.Contains(context.DocumentName.TrimStart('v').TrimStart('V')));
            if (version.IsNotNull())
            {
                var extensions = GetExtensionInfo(version).ToDictionary(x => x.key, x => x.openApiExtension);

                document.Info = new()
                {
                    Title = version.Title,
                    Version = version.Version,
                    Description = version.Description,
                    Contact = new OpenApiContact()
                    {
                        Email = version.ContactEmail,
                        Name = version.ContactName,
                    },
                    Extensions = extensions
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
        }

        private static IEnumerable<(string key, IOpenApiExtension openApiExtension)> GetExtensionInfo(OpenApiVersionInfo openApiInfo)
        {
            if (openApiInfo.Id is not null)
            {
                yield return ("x-api-id", new OpenApiString(openApiInfo.Id.ToString()));
            }

            if (openApiInfo.Audience is not null)
            {
                yield return ("x-audience", new OpenApiString(openApiInfo.Audience));
            }
        }
    }
}
