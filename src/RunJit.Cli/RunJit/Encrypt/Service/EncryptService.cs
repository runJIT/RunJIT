using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;
using RunJit.Cli.Services.Crypto;

namespace RunJit.Cli.RunJit.Encrypt
{
    public static class AddEncryptServiceExtension
    {
        public static void AddEncryptService(this IServiceCollection services)
        {
            services.AddCryptoService();

            services.AddSingletonIfNotExists<IEncryptService, EncryptService>();
        }
    }

    // Question: We have to think if this idea is good and we have to extend database interaction
    //           we have to think about an own database command like
    //           runjit database new Crypto
    internal interface IEncryptService
    {
        Task HandleAsync(EncryptParameters parameters);
    }

    internal class EncryptService(ICryptoService cryptoService,
                                  IConsoleService consoleService) : IEncryptService
    {
        public async Task HandleAsync(EncryptParameters parameters)
        {
            var decrypted = await cryptoService.EncryptAsync(parameters.Value).ConfigureAwait(false);
            consoleService.WriteSuccess(decrypted);
        }
    }

    internal record EncryptParameters(string Value);
}
