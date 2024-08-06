using System.CommandLine;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Update.UpdateBuildConfig
{
    public static class AddUpdateBuildConfigOptionsBuilderExtension
    {
        public static void AddUpdateBuildConfigOptionsBuilder(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<IUpdateBuildConfigOptionsBuilder, UpdateBuildConfigOptionsBuilder>();
        }
    }
    
    internal interface IUpdateBuildConfigOptionsBuilder
    {
        IEnumerable<Option> Build();
    }
    
    internal class UpdateBuildConfigOptionsBuilder : IUpdateBuildConfigOptionsBuilder
    {
        public IEnumerable<Option> Build()
        {
            yield break;
        }
    }
}
