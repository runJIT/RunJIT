using Argument.Check;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    public static class AddArgumentInterfaceBuilderExtension
    {
        public static void AddArgumentInterfaceBuilder(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<ArgumentInterfaceBuilder>();
        }
    }

    internal sealed class ArgumentInterfaceBuilder 
    {
        private const string Template =
            @"
using System.CommandLine;

namespace $namespace$
{    
    internal interface I$command-name$ArgumentBuilder
    {
        System.CommandLine.Argument Build();
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

            var newTemplate = Template.Replace("$projectName$", projectName)
                                      .Replace("$namespace$", currentNamespace)
                                      .Replace("$command-name$", parameterInfo.NormalizedName);

            return newTemplate;
        }
    }
}
