using Argument.Check;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using Solution.Parser.CSharp;

namespace RunJit.Cli.Generate.DotNetTool
{
    internal static class AddCommandServiceInterfaceBuilderExtension
    {
        internal static void AddCommandServiceInterfaceBuilder(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<CommandServiceInterfaceBuilder>();
        }
    }

    internal sealed class CommandServiceInterfaceBuilder
    {
        private const string Template =
            @"
namespace $namespace$
{    
    internal interface I$command-name$Handler
    {       
        Task HandleAsync($command-name$Parameters parameters);
    }
}";

        public string Build(string project,
                            Models.CommandInfo parameterInfo,
                            string nameSpace)
        {
            Throw.IfNullOrWhiteSpace(project);
            Throw.IfNullOrWhiteSpace(nameSpace);

            var currentNamespace = $"{nameSpace}.Service";

            var newTemplate = Template.Replace("$command-name$", parameterInfo.NormalizedName)
                                      .Replace("$namespace$", currentNamespace)
                                      .Replace("$project-name$", project);

            return newTemplate.FormatSyntaxTree();
        }
    }
}
