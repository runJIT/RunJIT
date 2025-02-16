using System.Diagnostics.CodeAnalysis;
using Extensions.Pack;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace $ProjectName$.IdentityProvider
{
    public static class AuthenticationBuilderExtensions
    {
        public static void AddCognito(this IServiceCollection services,
                                      ConfigurationManager configuration)
        {
            services.AddAwsCognitoSettings(configuration);
            
            var settings = configuration.GetSettings<AwsCognitoSettings>();
            
            services.AddSingletonIfNotExists(settings);
            var authenticationBuilder = services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);

            authenticationBuilder.AddJwtBearer(options =>
                                               {
                                                   options.Authority = settings.Authority;
                                                   options.Audience = settings.Audience;

                                                   options.TokenValidationParameters = new TokenValidationParameters
                                                   {
                                                       ValidateIssuer = true,
                                                       ValidateAudience = false,
                                                       ValidateLifetime = true,
                                                       ValidateIssuerSigningKey = true,
                                                       ValidIssuer = settings.Authority,
                                                       ValidAudience = settings.Audience,
                                                       IssuerSigningKeyResolver = [SuppressMessage("ReSharper", "UnusedParameter.Local")] (token,
                                                                                                                                          securityToken,
                                                                                                                                          kid,
                                                                                                                                          validationParameters) =>
                                                                                  {
                                                                                      var client = new HttpClient();
                                                                                      var response = client.GetAsync($"{settings.Authority}/.well-known/jwks.json").Result;
                                                                                      var keys = response.Content.ReadAsStringAsync().Result;
                                                                                      var jsonWebKeySet = new JsonWebKeySet(keys);

                                                                                      return jsonWebKeySet.GetSigningKeys();
                                                                                  }
                                                   };
                                               });
        }
    }
}
