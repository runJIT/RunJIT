using System.Text;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.Services.Crypto
{
    public static class AddCryptoServiceExtension
    {
        public static void AddCryptoService(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<ICryptoService, CryptoService>();
        }
    }

    // Simple crypto service for encrypting and decrypting strings.
    public interface ICryptoService
    {
        Task<string> EncryptAsync(string clearText);

        Task<string> DecryptAsync(string encrypted);
    }

    internal sealed class CryptoService : ICryptoService
    {
        public Task<string> EncryptAsync(string clearText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(clearText);
            var base64String = Convert.ToBase64String(plainTextBytes);

            return Task.FromResult(base64String);
        }

        public Task<string> DecryptAsync(string encrypted)
        {
            var base64EncodedBytes = Convert.FromBase64String(encrypted);
            var result = Encoding.UTF8.GetString(base64EncodedBytes);

            return Task.FromResult(result);
        }
    }
}
