using Argument.Check;
using Extensions.Pack;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal sealed class NewOptionExpressionBuilderWithArgument : INewOptionExpressionBuilder
    {
        private const string OptionArgumentTemplate =
            @"Option([""$option-name$"", ""$option-alias$""], ""$option-description$"")
            {
                Required = $required-value$,
                Argument = new Argument<$type$>(""$option-argument-name$"")
                {
                    Description = ""$argument-description$""
                }
            }";

        public string Build(OptionInfo optionInfo)
        {
            Throw.IfNull(() => optionInfo);

            var newTemplate = OptionArgumentTemplate.Replace("$option-name$", optionInfo.Value)
                                                    .Replace("$option-alias$", optionInfo.Alias)
                                                    .Replace("$option-argument-name$", global::Extensions.Pack.StringExtensions.FirstCharToLower((string)optionInfo.NormalizedName))
                                                    .Replace("$option-description$", optionInfo.Description)
                                                    .Replace("$required-value$", optionInfo.IsIsRequired.ToString().ToLower())
                                                    .Replace("$type$", optionInfo.Argument.OptimizedType)
                                                    .Replace("$argument-description$", optionInfo.Argument.Description);

            return newTemplate;
        }

        public bool IsBuilderFor(OptionInfo optionInfo)
        {
            Throw.IfNull(() => optionInfo);

            return ObjectExtensions.IsNotNull((object?)optionInfo.Argument);
        }
    }
}
