using Argument.Check;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    public static class AddArgumentBuilderExtension
    {
        public static void AddArgumentBuilder(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<ArgumentBuilder>();
        }
    }

    internal sealed class ArgumentBuilder 
    {
        private const string Template =
            @"
namespace $namespace$
{    
    internal sealed class $command-name$ArgumentBuilder : I$command-name$ArgumentBuilder
    {                                        
        public System.CommandLine.Argument Build()
        {
            var argument = new System.CommandLine.Argument<$type$>()
            {
                Name = ""$argument-name$"",
                Description = ""$argument-description$""
            };
            
            return argument;
        }
    }
}";

        public string Build(string projectName,
                            CommandInfo parameterInfo,
                            string nameSpace)
        {
            Throw.IfNullOrWhiteSpace(projectName);
            Throw.IfNull(() => parameterInfo);
            Throw.IfNullOrWhiteSpace(nameSpace);

            var currentNamespace = $"{nameSpace}.Arguments";

            var newTemplate = Template.Replace("$project-name$", projectName)
                                      .Replace("$command-name$", parameterInfo.NormalizedName)
                                      .Replace("$argument-name$", parameterInfo.Argument.Name)
                                      .Replace("$namespace$", currentNamespace)
                                      .Replace("$type$", parameterInfo.Argument.OptimizedType)
                                      .Replace("$argument-description$", parameterInfo.Argument.Description);

            return newTemplate;
        }
    }
}
