namespace RunJit.Cli.Models.Validation
{
    internal class ArgumentInfoValidationResult(ArgumentInfo source,
                                                string errors) : GenericValidationResult<ArgumentInfo>(source, errors);
}
