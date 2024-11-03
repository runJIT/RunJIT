using Argument.Check;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using Solution.Parser.CSharp;

namespace RunJit.Cli.Generate.DotNetTool
{
    internal static class AddParameterWithArgsOrOptionsClassBuilderExtension
    {
        internal static void AddParameterWithArgsOrOptionsClassBuilder(this IServiceCollection services)
        {
            services.AddConstructorArgumentBuilder();

            services.AddSingletonIfNotExists<IParameterSpecificClassBuilder, ParameterWithArgsOrOptionsClassBuilder>();
        }
    }

    internal sealed class ParameterWithArgsOrOptionsClassBuilder : IParameterSpecificClassBuilder
    {
        private const string Template =
            @"
namespace $namespace$
{    
    internal sealed record $command-name$Parameters($primary-ctor-arguments$);
}";

        private const string CtorArgument = @"$type$ $argName$";

        private readonly PrimaryConstructorArgumentBuilder _primaryConstructorArgumentBuilder;

        public ParameterWithArgsOrOptionsClassBuilder(PrimaryConstructorArgumentBuilder primaryConstructorArgumentBuilder)
        {
            Throw.IfNull(() => primaryConstructorArgumentBuilder);

            _primaryConstructorArgumentBuilder = primaryConstructorArgumentBuilder;
        }

        public string Build(string projectName,
                            Models.CommandInfo parameterInfo,
                            string nameSpace)
        {
            Throw.IfNullOrWhiteSpace(projectName);
            Throw.IfNull(() => parameterInfo);
            Throw.IfNullOrWhiteSpace(nameSpace);

            var ctorArguments = _primaryConstructorArgumentBuilder.Build(parameterInfo).ToList();
            var properties = BuildProperties(ctorArguments).ToList();
            var propertyString = BuildPropertyString(properties);

            var primaryCtroArguments = BuildArguments(ctorArguments);
            var propertyInitializer = BuildPropertyInitializerString(ctorArguments, properties);

            var newTemplate = Template.Replace("$primary-ctor-arguments$", primaryCtroArguments)
                                      .Replace("$agrument-to-properties$", propertyInitializer)
                                      .Replace("$properties$", propertyString)
                                      .Replace("$projectName$", projectName)
                                      .Replace("$namespace$", nameSpace)
                                      .Replace("$command-name$", parameterInfo.NormalizedName);

            return newTemplate.FormatSyntaxTree();
        }

        public bool IsThisBuilderFor(Models.CommandInfo parameterInfo)
        {
            Throw.IfNull(() => parameterInfo);

            return parameterInfo.Argument.IsNotNull() || parameterInfo.Options.Any();
        }

        private string BuildPropertyString(IEnumerable<Models.Property> properties)
        {
            var result = properties.Select(p => $"        public {p.Type} {p.Name} {{ get; }}").Flatten(Environment.NewLine);

            return result;
        }

        private string BuildPropertyInitializerString(IEnumerable<Models.CtorArgument> arguments,
                                                      IEnumerable<Models.Property> properties)
        {
            var result = BuildPropertyInitializer(arguments, properties);

            return result.Flatten(Environment.NewLine);
        }

        private IEnumerable<string> BuildPropertyInitializer(IEnumerable<Models.CtorArgument> arguments,
                                                             IEnumerable<Models.Property> properties)
        {
            foreach (var property in properties)
            {
                var argument = arguments.Single(a => a.Name.ToLower().EqualsTo(property.Name.ToLower()));

                yield return $"            {property.Name} = {argument.Name};";
            }
        }

        private string BuildArguments(IEnumerable<Models.CtorArgument> ctorArguments)
        {
            var result = ctorArguments.Select(arg => CtorArgument.Replace("$type$", arg.Type).Replace("$argName$", arg.Name.FirstCharToUpper())).Flatten(", ");

            return result;
        }

        private IEnumerable<Models.Property> BuildProperties(IEnumerable<Models.CtorArgument> ctorArguments)
        {
            foreach (var argument in ctorArguments)
            {
                yield return new Models.Property(argument.Type, argument.NormalizedName);
            }
        }
    }
}
