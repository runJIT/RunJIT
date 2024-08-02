namespace RunJit.Cli.Models.Validation
{
    internal class ObjectValidationResult(object source,
                                          string errors) : ValidationResult(errors)
    {
        public object Source { get; } = source;
    }
}
