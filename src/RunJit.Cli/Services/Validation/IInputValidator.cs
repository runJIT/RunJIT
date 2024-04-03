using RunJit.Cli.Models.Validation;

namespace RunJit.Cli
{
    public interface IInputValidator
    {
        ValidationResult Validate(string value);
    }
}
