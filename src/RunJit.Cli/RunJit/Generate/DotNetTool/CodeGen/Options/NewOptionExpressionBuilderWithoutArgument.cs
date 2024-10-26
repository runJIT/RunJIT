using Argument.Check;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using Solution.Parser.CSharp;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    public static class AddNewOptionExpressionBuilderWithoutArgumentExtension
    {
        public static void AddNewOptionExpressionBuilderWithoutArgument(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<INewOptionExpressionBuilder, NewOptionExpressionBuilderWithoutArgument>();
        }
    }

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

            return newTemplate.FormatSyntaxTree();
        }

        public bool IsBuilderFor(OptionInfo optionInfo)
        {
            Throw.IfNull(() => optionInfo);

            return optionInfo.Argument.IsNull();
        }
    }
}
