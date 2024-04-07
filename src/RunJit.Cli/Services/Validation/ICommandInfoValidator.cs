using RunJit.Cli.Models;

namespace RunJit.Cli
{
    internal interface ICommandInfoValidator
    {
        IEnumerable<ValidationResult> Validate(CommandInfo commandInfo);
    }
}
