﻿using Argument.Check;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    public static class AddOptionMethodsBuilderExtension
    {
        public static void AddOptionMethodsBuilder(this IServiceCollection services)
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
