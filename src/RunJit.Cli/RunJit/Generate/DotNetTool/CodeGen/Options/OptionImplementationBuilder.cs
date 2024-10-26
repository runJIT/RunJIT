﻿using Argument.Check;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    public static class AddOptionImplementationBuilderExtension
    {
        public static void AddOptionImplementationBuilder(this IServiceCollection services)
        {
            services.AddOptionMethodsBuilder();

            services.AddSingletonIfNotExists<OptionImplementationBuilder>();
        }
    }

    internal sealed class OptionImplementationBuilder
    {
        private const string Template =
            @"using System.CommandLine; 

namespace $namespace$
{    
    internal sealed class $command-name$OptionsBuilder
    {
        public IEnumerable<Option> Build()
        {   
$yield-option$
        }

$build-option-method$                                       
    }
}";

        private readonly OptionMethodsBuilder _optionMethodsBuilder;

        public OptionImplementationBuilder(OptionMethodsBuilder optionMethodsBuilder)
        {
            Throw.IfNull(() => optionMethodsBuilder);

            _optionMethodsBuilder = optionMethodsBuilder;
        }

        public string Build(string projectName,
                            CommandInfo parameterInfo,
                            string nameSpace)
        {
            Throw.IfNullOrWhiteSpace(projectName);
            Throw.IfNull(() => parameterInfo);
            Throw.IfNullOrWhiteSpace(nameSpace);

            var currentNamespace = $"{nameSpace}.Options";
            var optionsMethods = _optionMethodsBuilder.Build(parameterInfo.Options).ToList();
            var optionsMethodAsString = optionsMethods.Select(m => m.MethodSyntax).Flatten(Environment.NewLine);
            var yieldStatements = optionsMethods.Select(m => $"            yield return {m.MethodName}();").Flatten(Environment.NewLine);

            var newTemplate = Template.Replace("$project-name$", projectName)
                                      .Replace("$command-name$", parameterInfo.NormalizedName)
                                      .Replace("$yield-option$", yieldStatements)
                                      .Replace("$namespace$", currentNamespace)
                                      .Replace("$build-option-method$", optionsMethodAsString);

            return newTemplate;
        }
    }
}
