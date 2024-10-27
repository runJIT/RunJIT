namespace RunJit.Cli.Models.Validation
{
    internal sealed class OptionInfoValidationResult(OptionInfo source,
                                              string errors) : GenericValidationResult<OptionInfo>(source, errors);
}
