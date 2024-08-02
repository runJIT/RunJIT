namespace RunJit.Cli.Models.Validation
{
    internal class CommandInfoValidationResult(CommandInfo source,
                                               string errors) : GenericValidationResult<CommandInfo>(source, errors);
}
