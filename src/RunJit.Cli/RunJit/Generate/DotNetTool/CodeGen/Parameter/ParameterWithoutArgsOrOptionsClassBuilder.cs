using Argument.Check;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using Solution.Parser.CSharp;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal static class AddParameterWithoutArgsOrOptionsClassBuilderExtension
    {
        internal static void AddParameterWithoutArgsOrOptionsClassBuilder(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<IParameterSpecificClassBuilder, ParameterWithoutArgsOrOptionsClassBuilder>();
        }
    }

    internal sealed class ParameterWithoutArgsOrOptionsClassBuilder : IParameterSpecificClassBuilder
    {
        private const string Template =
            @"
namespace $namespace$
{    
    internal sealed record $command-name$Parameters();
}";

        public string Build(string projectName,
                            CommandInfo parameterInfo,
                            string nameSpace)
        {
            Throw.IfNullOrWhiteSpace(projectName);
            Throw.IfNullOrWhiteSpace(nameSpace);

            var newTemplate = Template.Replace("$projectName$", projectName)
                                      .Replace("$namespace$", nameSpace)
                                      .Replace("$command-name$", parameterInfo.NormalizedName);

            return newTemplate.FormatSyntaxTree();
        }

        public bool IsThisBuilderFor(CommandInfo parameterInfo)
        {
            Throw.IfNull(() => parameterInfo);

            return parameterInfo.Argument.IsNull() && parameterInfo.Options.IsNullOrEmpty();
        }
    }
}
