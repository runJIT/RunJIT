namespace RunJit.Cli.Models.Validation
{
    internal sealed class DotNetToolNameValidationResult(DotNetToolName source,
                                                  string errors) : GenericValidationResult<DotNetToolName>(source, errors);
}
