using System.Text;
using Argument.Check;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal static class DotNetToolGenParametersExtensions
    {
        internal static string ToInfo(this DotNetToolParameters clientGenParameters)
        {
            Throw.IfNull(clientGenParameters);

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"{nameof(clientGenParameters.SolutionFile)} = {clientGenParameters.SolutionFile.FullName}");
            stringBuilder.AppendLine($"{nameof(clientGenParameters.UseVisualStudio)} = {clientGenParameters?.UseVisualStudio}");
            return stringBuilder.ToString();
        }
    }
}
