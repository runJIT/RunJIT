using System.Collections.Immutable;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal static class AddIntegrateIntoSourceSolutionValidatorExtension
    {
        internal static void AddIntegrateIntoSourceSolutionValidator(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<IntegrateIntoSourceSolutionValidator>();
        }
    }

    internal class IntegrateIntoSourceSolutionValidator : IInputValidator
    {
        public ValidationResult Validate(string value)
        {
            var errors = CollectErrors(value).Flatten(Environment.NewLine);
            return new ValidationResult(errors);
        }

        private IEnumerable<string> CollectErrors(string value)
        {
            if (value.IsNullOrWhiteSpace())
            {
                yield return $"Your option: '{value}' must not be null, empty or whitespace";
                yield break;
            }

            if (value.Contains(" "))
            {
                yield return $"Your option: '{value}' must not contains whitespace";
                yield break;
            }

            if (value.Any(c => char.IsDigit(c).IsFalse()))
            {
                yield return $"Your option: '{value}' must be a number";
            }

            if (int.TryParse(value, out var option).IsFalse())
            {
                yield return $"Your option: '{value}' was not a valid number";
            }

            // ToDo: small workaround.
            var immutableList = ImmutableList.Create(1, 2);
            if (immutableList.Contains(option).IsFalse())
            {
                yield return $"Your option: '{value}' was not an available option. Available options are: {immutableList.Select(number => number.ToInvariantString()).Flatten(",")}";
            }
        }
    }
}
