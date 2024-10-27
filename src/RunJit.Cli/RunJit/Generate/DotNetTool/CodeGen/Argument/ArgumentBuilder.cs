using Argument.Check;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using Solution.Parser.CSharp;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    public static class AddArgumentBuilderCodeExtension
    {
        public static void AddArgumentBuilderCodeGen(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<ArgumentBuilder>();
        }
    }

    internal sealed class ArgumentBuilder 
    {
        private const string Template =
            @"
using Extensions.Pack;

namespace $namespace$
{    
    internal static class Add$command-name$ArgumentBuilderExtension
    {
        internal static void Add$command-name$ArgumentBuilder(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<$command-name$ArgumentBuilder>();
        }
    }

    internal sealed class $command-name$ArgumentBuilder
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

            var newTemplate = Template.Replace("$project-name$", projectName)
                                      .Replace("$command-name$", parameterInfo.NormalizedName)
                                      .Replace("$argument-name$", parameterInfo.Argument?.Name)
                                      .Replace("$namespace$", nameSpace)
                                      .Replace("$type$", parameterInfo.Argument?.OptimizedType)
                                      .Replace("$argument-description$", parameterInfo.Argument?.Description);

            return newTemplate.FormatSyntaxTree();
        }
    }
}
