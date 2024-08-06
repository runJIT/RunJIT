using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Update.UpdateBuildConfig
{
    public static class AddUpdateBuildConfigArgumentsBuilderExtension
    {
        public static void AddUpdateBuildConfigArgumentsBuilder(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<IUpdateBuildConfigArgumentsBuilder, UpdateBuildConfigArgumentsBuilder>();
        }
    }
    
    internal interface IUpdateBuildConfigArgumentsBuilder
    {
        IEnumerable<System.CommandLine.Argument> Build();
    }
    
    internal class UpdateBuildConfigArgumentsBuilder : IUpdateBuildConfigArgumentsBuilder
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
