using Argument.Check;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal sealed class OptionInterfaceBuilder : IOptionInterfaceBuilder
    {
        private const string Template =
            @"using System.CommandLine;

namespace $namespace$
{    
    internal interface I$command-name$OptionsBuilder
    {
        IEnumerable<Option> Build();
    }
}";

        public string Build(string projectName,
                            CommandInfo parameterInfo,
                            string nameSpace)
        {
            Throw.IfNullOrWhiteSpace(projectName);
            Throw.IfNull(() => parameterInfo);
            Throw.IfNullOrWhiteSpace(nameSpace);

            var currentNamespace = $"{nameSpace}.Options";

            var newTemplate = Template.Replace("$projectName$", projectName)
                                      .Replace("$namespace$", currentNamespace)
                                      .Replace("$command-name$", parameterInfo.NormalizedName);

            return newTemplate;
        }
    }
}
