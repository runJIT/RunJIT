using DotNetTool.Service;
using Extensions.Pack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Auth0;
using RunJit.Cli.ErrorHandling;
using RunJit.Cli.RunJit;

namespace RunJit.Cli
{
    internal sealed class Startup
    {
        internal void ConfigureServices(IServiceCollection services,
                                        IConfiguration configuration)
        {
            // 1. Infrastructure
            services.AddDotNetCliArgumentFixer();
            services.AddErrorHandler();

            // 2. Security
            services.AddAuth0(configuration);

            // Domains
            // RunJit
            //  -> Rename
            //  -> Update
            services.AddRunJitCommandBuilder(configuration);

            // External libs
            var dotnetTool = DotNetToolFactory.Create();
            services.AddSingletonIfNotExists(dotnetTool);
        }
    }
}
