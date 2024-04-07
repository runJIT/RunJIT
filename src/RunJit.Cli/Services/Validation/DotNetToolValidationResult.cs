using Extensions.Pack;

namespace RunJit.Cli
{
    internal class DotNetToolValidationResult(IEnumerable<ValidationResult> result)
    {
        internal IEnumerable<ValidationResult> ValidationResults { get; } = result;

        internal bool HasErrors { get; } = result.Any(r => r.IsValid.IsNot());
    }
}
