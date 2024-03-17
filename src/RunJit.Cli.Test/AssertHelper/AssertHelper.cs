using System.Text;

namespace RunJit.Cli.Test.AssertHelper
{
    internal static class AssertHelper
    {
        internal static string ToErrorMessage(this IEnumerable<string> errors, string title)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(title);
            stringBuilder.AppendLine();

            errors.ToList().ForEach(error => stringBuilder.AppendLine($"- {error}"));
            return stringBuilder.ToString();
        }
    }
}
