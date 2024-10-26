﻿using Argument.Check;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    public static class AddParameterWithArgsOrOptionsClassBuilderExtension
    {
        public static void AddParameterWithArgsOrOptionsClassBuilder(this IServiceCollection services)
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
    internal sealed record $command-name$Parameters($ctor-arguments$);
}";

        private const string CtorArgument = @"$type$ $argName$";

        private readonly PrimaryConstructorArgumentBuilder _primaryConstructorArgumentBuilder;

        public ParameterWithArgsOrOptionsClassBuilder(PrimaryConstructorArgumentBuilder primaryConstructorArgumentBuilder)
        {
            Throw.IfNull(() => primaryConstructorArgumentBuilder);

            _primaryConstructorArgumentBuilder = primaryConstructorArgumentBuilder;
        }

        public string Build(string projectName,
                            CommandInfo parameterInfo,
                            string nameSpace)
        {
            Throw.IfNullOrWhiteSpace(projectName);
            Throw.IfNull(() => parameterInfo);
            Throw.IfNullOrWhiteSpace(nameSpace);

            var ctorArguments = _primaryConstructorArgumentBuilder.Build(parameterInfo).ToList();
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

            return parameterInfo.Argument.IsNotNull() || parameterInfo.Options.Any();
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
