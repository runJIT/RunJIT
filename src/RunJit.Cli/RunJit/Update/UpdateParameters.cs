using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Update
{
    public static class AddUpdateParametersExtension
    {
        public static void AddUpdateParameters(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<UpdateParameters>();
        }
    }

    internal record UpdateParameters(string Version);
}
