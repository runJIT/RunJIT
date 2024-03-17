using RunJit.Cli.Models;
using RunJit.Cli.Models.Validation;

namespace RunJit.Cli
{
    internal interface ICommandInfoValidator
    {
        IEnumerable<ValidationResult> Validate(CommandInfo commandInfo);
    }
}
