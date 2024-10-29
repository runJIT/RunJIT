using AspNetCore.Simple.Sdk.Authentication.Auth0;
using AspNetCore.Simple.Sdk.Mediator;
using Extensions.Pack;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.Auth0
{
    internal static class AddAuth0Extension
    {
        internal static void AddAuth0(this IServiceCollection services,
                                    IConfiguration configuration)
        {
            services.AddAuth0Settings(configuration);

            services.AddMediatR(config =>
                                {
                                    config.RegisterServicesFromAssembly(typeof(GetAuth0TokenFor).Assembly);
                                });
        }
    }

    internal record GetTokenByStorageCache() : IQuery<Auth0Token>;

    internal sealed class GetTokenByStorageCacheHandler(IMediator mediator,
                                                 AspNetCore.Simple.Sdk.Authentication.Auth0.Auth0 authSettings) : IQueryHandler<GetTokenByStorageCache, Auth0Token>
    {
        public async Task<Auth0Token> Handle(GetTokenByStorageCache request,
                                             CancellationToken cancellationToken)
        {
            // Get the path to the current user's folder
            string userFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            // Define the path to the JSON file
            string jsonFilePath = Path.Combine(userFolderPath, "RunJit.Cli", "secrets.json");

            // Write the JSON string to the file
            var fileInfo = new FileInfo(jsonFilePath);

            // Create if not exists
            if (fileInfo.Directory!.NotExists())
            {
                fileInfo.Directory!.Create();
            }

            if (fileInfo.NotExists())
            {
                await File.WriteAllTextAsync(jsonFilePath, "{}", cancellationToken).ConfigureAwait(false);
            }

            var secretFileContent = await File.ReadAllTextAsync(jsonFilePath, cancellationToken).ConfigureAwait(false);
            var auth0Token = secretFileContent.FromJsonStringOrDefault<Auth0Token>();

            if (auth0Token.IsNull())
            {
                auth0Token = await mediator.SendAsync(new GetAuth0TokenFor(authSettings), cancellationToken).ConfigureAwait(false);
                await File.WriteAllTextAsync(fileInfo.FullName, auth0Token.ToJsonIntended(), cancellationToken).ConfigureAwait(false);
            }

            if (auth0Token.IsNotNull() && auth0Token.ExpiresOnUtc.Subtract(DateTimeOffset.UtcNow).TotalMinutes < 1)
            {
                auth0Token = await mediator.SendAsync(new GetAuth0TokenFor(authSettings), cancellationToken).ConfigureAwait(false);
                await File.WriteAllTextAsync(fileInfo.FullName, auth0Token.ToJsonIntended(), cancellationToken).ConfigureAwait(false);
            }

            // If all fine we already use existing token.
            return auth0Token;
        }
    }
}
