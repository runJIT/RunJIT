using Argument.Check;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    public static class AddRootCommandInterfaceBuilderExtension
    {
        public static void AddRootCommandInterfaceBuilder(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<RootCommandInterfaceBuilder>();
        }
    }

    internal sealed class RootCommandInterfaceBuilder 
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

        public string Build(string project,
                            CommandInfo parameterInfo,
                            string nameSpace)
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
