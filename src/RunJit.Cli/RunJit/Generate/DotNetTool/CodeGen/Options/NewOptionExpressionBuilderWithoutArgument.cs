using Argument.Check;
using Extensions.Pack;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal sealed class NewOptionExpressionBuilderWithoutArgument : INewOptionExpressionBuilder
    {
        private const string OptionTemplate =
            @"Option([""$option-name$"", ""$option-alias$""], ""$option-description$"")
            {
                Required = $required-value$
            }";

        public string Build(OptionInfo optionInfo)
        {
            Throw.IfNull(() => optionInfo);

            var newTemplate = OptionTemplate.Replace("$option-name$", optionInfo.Value)
                                            .Replace("$option-alias$", optionInfo.Alias)
                                            .Replace("$option-description$", optionInfo.Description)
                                            .Replace("$required-value$", optionInfo.IsIsRequired.ToString().ToLower());

            return newTemplate;
        }

        public bool IsBuilderFor(OptionInfo optionInfo)
        {
            Throw.IfNull(() => optionInfo);

            return ObjectExtensions.IsNull((object?)optionInfo.Argument);
        }
    }
}
