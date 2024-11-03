using Argument.Check;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using Solution.Parser.CSharp;

namespace RunJit.Cli.Generate.DotNetTool
{
    internal static class AddNewOptionExpressionBuilderWithArgumentExtension
    {
        internal static void AddNewOptionExpressionBuilderWithArgument(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<INewOptionExpressionBuilder, NewOptionExpressionBuilderWithArgument>();
        }
    }

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

        public string Build(Models.OptionInfo optionInfo)
        {
            Throw.IfNull(() => optionInfo);

            var newTemplate = OptionArgumentTemplate.Replace("$option-name$", optionInfo.Value)
                                                    .Replace("$option-alias$", optionInfo.Alias)
                                                    .Replace("$option-argument-name$", ((string)optionInfo.NormalizedName).FirstCharToLower())
                                                    .Replace("$option-description$", optionInfo.Description)
                                                    .Replace("$required-value$", optionInfo.IsIsRequired.ToString().ToLower())
                                                    .Replace("$type$", optionInfo.Argument?.OptimizedType)
                                                    .Replace("$argument-description$", optionInfo.Argument?.Description);

            return newTemplate.FormatSyntaxTree();
        }

        public bool IsBuilderFor(Models.OptionInfo optionInfo)
        {
            Throw.IfNull(() => optionInfo);

            return optionInfo.Argument.IsNotNull();
        }
    }
}
