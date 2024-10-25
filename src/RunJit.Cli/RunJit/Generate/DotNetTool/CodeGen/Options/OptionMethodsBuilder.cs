using Argument.Check;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal sealed class OptionMethodsBuilder : IOptionMethodsBuilder
    {
        private const string OptionMethodTemplate =
            @"        private Option Build$option-name$Option()
        {
            return new $option$;
        }";

        private readonly INewOptionExpressionService _newOptionExpressionService;

        public OptionMethodsBuilder(INewOptionExpressionService newOptionExpressionService)
        {
            Throw.IfNull(() => newOptionExpressionService);

            _newOptionExpressionService = newOptionExpressionService;
        }

        public IEnumerable<MethodInfo> Build(IEnumerable<OptionInfo> options)
        {
            Throw.IfNull(() => options);

            foreach (var option in options)
            {
                var newOptionStatement = _newOptionExpressionService.Build(option);
                var newMethod = OptionMethodTemplate.Replace("$option$", newOptionStatement).Replace("$option-name$", option.NormalizedName);
                var methodName = $"Build{option.NormalizedName}Option";

                yield return new MethodInfo(methodName, newMethod);
            }
        }
    }
}
