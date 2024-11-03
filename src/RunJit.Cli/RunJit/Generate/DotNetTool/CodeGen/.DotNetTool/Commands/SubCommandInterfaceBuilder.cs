using Argument.Check;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using Solution.Parser.CSharp;

namespace RunJit.Cli.Generate.DotNetTool
{
    internal static class AddSubCommandInterfaceBuilderExtension
    {
        internal static void AddSubCommandInterfaceBuilder(this IServiceCollection services)
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
                            Models.CommandInfo parameterInfo,
                            Models.CommandInfo? parent,
                            string nameSpace)
        {
            Throw.IfNullOrWhiteSpace(project);
            Throw.IfNullOrWhiteSpace(nameSpace);

            var newTemplate = Template.Replace("$command-name$", parameterInfo.NormalizedName)
                                      .Replace("$namespace$", nameSpace)
                                      .Replace("$project-name$", project);

            return newTemplate.FormatSyntaxTree();
        }
    }
}
