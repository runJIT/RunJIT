using Argument.Check;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.Generate.DotNetTool
{
    internal static class AddNewOptionExpressionServiceExtension
    {
        internal static void AddNewOptionExpressionService(this IServiceCollection services)
        {
            services.AddNewOptionExpressionBuilderWithArgument();
            services.AddNewOptionExpressionBuilderWithoutArgument();

            services.AddSingletonIfNotExists<NewOptionExpressionService>();
        }
    }

    internal sealed class NewOptionExpressionService
    {
        private readonly IEnumerable<INewOptionExpressionBuilder> _newOptionExpressionBuilders;

        public NewOptionExpressionService(IEnumerable<INewOptionExpressionBuilder> newOptionExpressionBuilders)
        {
            Throw.IfNullOrEmpty(newOptionExpressionBuilders);

            _newOptionExpressionBuilders = newOptionExpressionBuilders;
        }

        public string Build(Models.OptionInfo optionInfo)
        {
            Throw.IfNull(() => optionInfo);

            var builder = _newOptionExpressionBuilders.Single(b => b.IsBuilderFor(optionInfo));

            return builder.Build(optionInfo);
        }
    }
}
