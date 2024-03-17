using Extensions.Pack;
using RunJit.Cli.Models;
using RunJit.Cli.Models.Validation;

namespace RunJit.Cli
{
    internal class DotNetToolValidationResult(IEnumerable<ValidationResult> result)
    {
        internal IEnumerable<ValidationResult> ValidationResults { get; } = result;

        internal bool HasErrors { get; } = result.Any(r => r.IsValid.IsNot());
    }
}
