using Argument.Check;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal sealed class RootCommandInterfaceBuilder : IRootCommandInterfaceBuilder
    {
        private const string Template =
@"using System.CommandLine;

namespace $namespace$
{    
    internal interface I$command-name$CommandBuilder
    {
        Command Build();
    }
}";

        public string Build(string project, CommandInfo parameterInfo, string nameSpace)
        {
            Throw.IfNullOrWhiteSpace(project);
            Throw.IfNull(() => parameterInfo);
            Throw.IfNullOrWhiteSpace(nameSpace);

            var newTemplate = Template.Replace("$command-name$", parameterInfo.NormalizedName)
                .Replace("$namespace$", nameSpace)
                .Replace("$project-name$", project);

            return newTemplate;
        }
    }
}
