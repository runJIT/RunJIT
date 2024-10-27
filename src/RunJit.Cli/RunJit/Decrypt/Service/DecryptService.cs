using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;
using RunJit.Cli.Services.Crypto;

namespace RunJit.Cli.RunJit.Decrypt
{
    public static class AddDecryptServiceExtension
    {
        public static void AddDecryptService(this IServiceCollection services)
        {
            services.AddCryptoService();

            services.AddSingletonIfNotExists<IDecryptService, DecryptService>();
        }
    }

    // Question: We have to think if this idea is good and we have to extend database interaction
    //           we have to think about an own database command like
    //           runjit database new Crypto
    internal interface IDecryptService
    {
        Task HandleAsync(DecryptParameters parameters);
    }

    internal sealed class DecryptService(ICryptoService cryptoService,
                                  ConsoleService consoleService) : IDecryptService
    {
        public async Task HandleAsync(DecryptParameters parameters)
        {
            var decrypted = await cryptoService.DecryptAsync(parameters.Value).ConfigureAwait(false);
            consoleService.WriteSuccess(decrypted);
        }
    }

    internal record DecryptParameters(string Value);
}
