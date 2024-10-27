using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal static class AddSolutionFileValidatorExtension
    {
        internal static void AddSolutionFileValidator(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<SolutionFileValidator>();
        }
    }

    internal sealed class SolutionFileValidator : IInputValidator
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
                yield return $"The solution file path: '{value}' must not be null, empty or whitespace";

                yield break;
            }

            if (value.Contains(" "))
            {
                yield return $"The solution file path: '{value}' must not contains whitespace";

                yield break;
            }

            var solutionFile = new FileInfo(value);

            if (solutionFile.Exists.IsFalse())
            {
                yield return $"The solution file path: '{solutionFile.FullName}' does not exists";
            }
        }
    }
}
