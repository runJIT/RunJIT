using Argument.Check;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal static class AddOptionMethodsBuilderExtension
    {
        internal static void AddOptionMethodsBuilder(this IServiceCollection services)
        {
            services.AddNewOptionExpressionService();

            services.AddSingletonIfNotExists<OptionMethodsBuilder>();
        }
    }

    internal sealed class OptionMethodsBuilder
    {
        private const string OptionMethodTemplate =
            @"        private Option Build$option-name$Option()
        {
            return new $option$;
        }";

        private readonly NewOptionExpressionService _newOptionExpressionService;

        public OptionMethodsBuilder(NewOptionExpressionService newOptionExpressionService)
        {
            Throw.IfNull(() => newOptionExpressionService);

            _newOptionExpressionService = newOptionExpressionService;
        }

        public IEnumerable<Models.MethodInfo> Build(IEnumerable<Models.OptionInfo> options)
        {
            Throw.IfNull(() => options);

            foreach (var option in options)
            {
                var newOptionStatement = _newOptionExpressionService.Build(option);
                var newMethod = OptionMethodTemplate.Replace("$option$", newOptionStatement).Replace("$option-name$", option.NormalizedName);
                var methodName = $"Build{option.NormalizedName}Option";

                yield return new Models.MethodInfo(methodName, newMethod);
            }
        }
    }
}
