using RunJit.Cli.Models;

namespace RunJit.Cli.Services
{
    internal interface ICommandInfoValidator
    {
        IEnumerable<ValidationResult> Validate(CommandInfo commandInfo);
    }
}
