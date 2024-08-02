namespace RunJit.Cli.Models.Validation
{
    internal class DotNetToolNameValidationResult(DotNetToolName source,
                                                  string errors) : GenericValidationResult<DotNetToolName>(source, errors);
}
