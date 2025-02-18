using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Update
{
    internal static class AddUpdateParametersExtension
    {
        internal static void AddUpdateParameters(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<UpdateParameters>();
        }
    }

    internal sealed record UpdateParameters(string Version);
}
