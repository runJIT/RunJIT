using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Update.BuildConfig
{
    internal static class AddUpdateBuildConfigArgumentsBuilderExtension
    {
        internal static void AddUpdateBuildConfigArgumentsBuilder(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<IUpdateBuildConfigArgumentsBuilder, UpdateBuildConfigArgumentsBuilder>();
        }
    }
    
    internal interface IUpdateBuildConfigArgumentsBuilder
    {
        IEnumerable<System.CommandLine.Argument> Build();
    }
    
    internal sealed class UpdateBuildConfigArgumentsBuilder : IUpdateBuildConfigArgumentsBuilder
    {
        public IEnumerable<System.CommandLine.Argument> Build()
        {
            yield break;
        }

        //public System.CommandLine.Argument BuildSolutionArgument()
        //{
        //    return new System.CommandLine.Argument<string>()
        //           {
        //               Name = "solutionFile",
                       
        //           };
        //}
    }
}
