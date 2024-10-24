using Argument.Check;
using Extensions.Pack;
using RunJit.Cli.RunJit.Generate.DotNetTool.CodeGen.Models;

namespace RunJit.Cli.RunJit.Generate.DotNetTool.CodeGen.Options
{
    internal sealed class OptionImplementationBuilder : IOptionImplementationBuilder
    {
        private const string Template =
@"using System.CommandLine; 

namespace $namespace$
{    
    internal sealed class $command-name$OptionsBuilder : I$command-name$OptionsBuilder
    {
        public IEnumerable<Option> Build()
        {   
$yield-option$
        }

$build-option-method$                                       
    }
}";

        private readonly IOptionMethodsBuilder _optionMethodsBuilder;

        public OptionImplementationBuilder(IOptionMethodsBuilder optionMethodsBuilder)
        {
            Throw.IfNull(() => optionMethodsBuilder);

            _optionMethodsBuilder = optionMethodsBuilder;
        }

        public string Build(string projectName, CommandInfo parameterInfo, string nameSpace)
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
