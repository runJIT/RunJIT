using Argument.Check;
using Extensions.Pack;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal sealed class ParameterWithArgsOrOptionsClassBuilder : IParameterSpecificClassBuilder
    {
        private const string Template =
            @"
namespace $namespace$
{    
    internal sealed class $command-name$Parameters
    {
        public $command-name$Parameters($ctor-arguments$)
        {
$agrument-to-properties$
        }

$properties$
    }
}";

        private const string CtorArgument = @"$type$ $argName$";

        private readonly IConstructorArgumentBuilder _constructorArgumentBuilder;

        public ParameterWithArgsOrOptionsClassBuilder(IConstructorArgumentBuilder constructorArgumentBuilder)
        {
            Throw.IfNull(() => constructorArgumentBuilder);

            _constructorArgumentBuilder = constructorArgumentBuilder;
        }

        public string Build(string projectName,
                            CommandInfo parameterInfo,
                            string nameSpace)
        {
            Throw.IfNullOrWhiteSpace(projectName);
            Throw.IfNull(() => parameterInfo);
            Throw.IfNullOrWhiteSpace(nameSpace);

            var ctorArguments = _constructorArgumentBuilder.Build(parameterInfo).ToList();
            var properties = BuildProperties(ctorArguments).ToList();
            var propertyString = BuildPropertyString(properties);

            var argumentString = BuildArguments(ctorArguments);
            var propertyInitializer = BuildPropertyInitializerString(ctorArguments, properties);

            var newTemplate = Template.Replace("$ctor-arguments$", argumentString)
                                      .Replace("$agrument-to-properties$", propertyInitializer)
                                      .Replace("$properties$", propertyString)
                                      .Replace("$projectName$", projectName)
                                      .Replace("$namespace$", nameSpace)
                                      .Replace("$command-name$", parameterInfo.NormalizedName);

            return newTemplate;
        }

        public bool IsThisBuilderFor(CommandInfo parameterInfo)
        {
            Throw.IfNull(() => parameterInfo);

            return ObjectExtensions.IsNotNull((object?)parameterInfo.Argument) || parameterInfo.Options.Any();
        }

        private string BuildPropertyString(IEnumerable<Property> properties)
        {
            var result = properties.Select(p => $"        public {p.Type} {p.Name} {{ get; }}").Flatten(Environment.NewLine);

            return result;
        }

        private string BuildPropertyInitializerString(IEnumerable<CtorArgument> arguments,
                                                      IEnumerable<Property> properties)
        {
            var result = BuildPropertyInitializer(arguments, properties);

            return result.Flatten(Environment.NewLine);
        }

        private IEnumerable<string> BuildPropertyInitializer(IEnumerable<CtorArgument> arguments,
                                                             IEnumerable<Property> properties)
        {
            foreach (var property in properties)
            {
                var argument = arguments.Single(a => a.Name.ToLower().EqualsTo(property.Name.ToLower()));

                yield return $"            {property.Name} = {argument.Name};";
            }
        }

        private string BuildArguments(IEnumerable<CtorArgument> ctorArguments)
        {
            var result = ctorArguments.Select(arg => CtorArgument.Replace("$type$", arg.Type).Replace("$argName$", arg.Name)).Flatten(", ");

            return result;
        }

        private IEnumerable<Property> BuildProperties(IEnumerable<CtorArgument> ctorArguments)
        {
            foreach (var argument in ctorArguments)
            {
                yield return new Property(argument.Type, argument.NormalizedName);
            }
        }
    }
}
