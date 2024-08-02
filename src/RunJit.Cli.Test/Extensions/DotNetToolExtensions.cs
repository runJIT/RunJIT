using System.Text;
using DotNetTool.Service;
using Extensions.Pack;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RunJit.Cli.Test.Extensions
{
    internal static class DotNetToolExtensions
    {
        public static async Task AssertRunAsync(this IDotNetTool dotNetTool,
                                                string dotnetTool,
                                                string arguments)
        {
            var stringBuilderStdOut = new StringBuilder();
            var stringBuilderErrorOut = new StringBuilder();

            var process = Process.StartProcess(dotnetTool, arguments, null,
                                               stdout => stringBuilderStdOut.AppendLine(stdout), error => stringBuilderErrorOut.AppendLine(error));

            await process.WaitForExitAsync().ConfigureAwait(false);

            var errors = stringBuilderErrorOut.ToString();
            var output = stringBuilderStdOut.ToString();

            Assert.AreEqual(0, process.ExitCode, errors.IsNullOrEmpty() ? output : errors);
        }
    }
}
