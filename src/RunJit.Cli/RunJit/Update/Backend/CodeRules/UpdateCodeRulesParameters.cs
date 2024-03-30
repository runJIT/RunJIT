using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Update.Backend.CodeRules
{
    public static class AddUpdateCodeRulesParametersExtension
    {
        public static void AddUpdateCodeRulesParameters(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<UpdateCodeRulesParameters>();
        }
    }

    internal record UpdateCodeRulesParameters(string SolutionFile, 
                                              string GitRepos, 
                                              string WorkingDirectory, 
                                              string IgnorePackages,
                                              string Branch);
}
