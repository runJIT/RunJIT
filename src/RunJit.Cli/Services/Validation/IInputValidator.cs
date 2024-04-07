using System.Diagnostics;
using Extensions.Pack;

namespace RunJit.Cli
{
    public interface IInputValidator
    {
        ValidationResult Validate(string value);
    }
    
    [DebuggerDisplay("Validate: '{" + nameof(IsValid) + "}")]
    public class ValidationResult(string errors)
    {
        public bool IsValid { get; } = errors.IsNullOrWhiteSpace();

        public string Errors { get; } = errors;
    }
}
