using Extensions.Pack;

namespace $ProjectName$.IdentityProvider
{
    public static class AddAwsCognitoSettingsExtension
    {
        internal static void AddAwsCognitoSettings(this IServiceCollection services,
                                                   IConfiguration configuration)
        {
            services.AddSingletonOption<AwsCognitoSettings>(configuration);
        }
    }

    internal sealed class AwsCognitoSettings
    {
        public string Audience { get; init; } = string.Empty;

        public string Authority { get; init; } = string.Empty;
    }
}
