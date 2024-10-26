using Argument.Check;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    public static class AddCommandServiceInterfaceBuilderExtension
    {
        public static void AddCommandServiceInterfaceBuilder(this IServiceCollection services)
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
    internal interface I$command-name$Service
    {       
        Task HandleAsync($command-name$Parameters parameters);
    }
}";

        public string Build(string project,
                            CommandInfo parameterInfo,
                            string nameSpace)
        {
            Throw.IfNullOrWhiteSpace(project);
            Throw.IfNullOrWhiteSpace(nameSpace);

            var currentNamespace = $"{nameSpace}.Service";

            var newTemplate = Template.Replace("$command-name$", parameterInfo.NormalizedName)
                                      .Replace("$namespace$", currentNamespace)
                                      .Replace("$project-name$", project);

            return newTemplate;
        }
    }
}
