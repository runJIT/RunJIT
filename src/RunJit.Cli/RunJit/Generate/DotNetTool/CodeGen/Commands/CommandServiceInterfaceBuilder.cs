using Argument.Check;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal sealed class CommandServiceInterfaceBuilder : ICommandServiceInterfaceBuilder
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

        public string Build(string project, CommandInfo parameterInfo, string nameSpace)
        {
            Throw.IfNullOrWhiteSpace(project);
            Throw.IfNull(() => parameterInfo);
            Throw.IfNullOrWhiteSpace(nameSpace);

            var currentNamespace = $"{nameSpace}.Service";
            var newTemplate = Template.Replace("$command-name$", parameterInfo.NormalizedName)
                .Replace("$namespace$", currentNamespace)
                .Replace("$project-name$", project);

            return newTemplate;
        }
    }
}
