namespace RunJit.Cli.Models.Validation
{
    internal class OptionInfoValidationResult(OptionInfo source, string errors) : GenericValidationResult<OptionInfo>(source, errors);
}
