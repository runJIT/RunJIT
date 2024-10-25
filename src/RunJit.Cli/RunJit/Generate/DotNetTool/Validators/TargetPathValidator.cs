using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal static class AddTargetPathValidatorExtension
    {
        internal static void AddTargetPathValidator(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<TargetPathValidator>();
        }
    }

    internal class TargetPathValidator : IInputValidator
    {
        public ValidationResult Validate(string value)
        {
            // No argument check here !
            // Throw.IfNullOrWhiteSpace(() => value);

            var errors = CollectErrors(value).Flatten(Environment.NewLine);

            return new ValidationResult(errors);
        }

        private IEnumerable<string> CollectErrors(string value)
        {
            if (value.IsNullOrWhiteSpace())
            {
                yield return $"The target path: '{value}' must not be null, empty or whitespace";

                yield break;
            }

            if (value.Contains(" "))
            {
                yield return $"TThe target path: '{value}' must not contains whitespace";

                yield break;
            }

            if (char.IsLetter(value.First()).IsFalse())
            {
                yield return $"The target path: '{value}' must start with a letter";
            }

            if (char.IsLetterOrDigit(value.Last()).IsFalse())
            {
                yield return $"The target path: '{value}' must end with a letter or digits";
            }

            if (Path.IsPathFullyQualified(value).IsFalse())
            {
                yield return $"The target path: '{value}' is not a valid path";
            }
        }
    }
}
