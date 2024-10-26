using Argument.Check;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    public static class AddOptionInterfaceBuilderExtension
    {
        public static void AddOptionInterfaceBuilder(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<OptionInterfaceBuilder>();
        }
    }

    internal sealed class OptionInterfaceBuilder
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
