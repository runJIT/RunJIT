using Argument.Check;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    public static class AddSubCommandInterfaceBuilderExtension
    {
        public static void AddSubCommandInterfaceBuilder(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<SubCommandInterfaceBuilder>();
        }
    }

    internal sealed class SubCommandInterfaceBuilder 
    {
        private const string Template =
            @"using System.CommandLine;

namespace $namespace$
{    
    internal interface I$command-name$SubCommandBuilder
    {
        Command Build();
    }
}";

        public string Build(string project,
                            CommandInfo parameterInfo,
                            CommandInfo parent,
                            string nameSpace)
        {
            Throw.IfNullOrWhiteSpace(project);
            Throw.IfNull(parameterInfo);
            Throw.IfNull(parent);
            Throw.IfNullOrWhiteSpace(nameSpace);

            var newTemplate = Template.Replace("$command-name$", parameterInfo.NormalizedName)
                                      .Replace("$namespace$", nameSpace)
                                      .Replace("$project-name$", project);

            return newTemplate;
        }
    }
}
