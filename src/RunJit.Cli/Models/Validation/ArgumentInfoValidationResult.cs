namespace RunJit.Cli.Models.Validation
{
    internal sealed class ArgumentInfoValidationResult(ArgumentInfo source,
                                                       string errors) : GenericValidationResult<ArgumentInfo>(source, errors);
}
