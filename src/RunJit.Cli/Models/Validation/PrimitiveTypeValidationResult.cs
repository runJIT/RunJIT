using System.Diagnostics;

namespace RunJit.Cli.Models.Validation
{
    [DebuggerDisplay("Validate: '{" + nameof(IsValid) + "}")]
    public class PrimitiveTypeValidationResult(string errors, Type? type, string alias) : ValidationResult(errors)
    {
        public Type? Type { get; } = type;

        public string Alias { get; } = alias;
    }
}
