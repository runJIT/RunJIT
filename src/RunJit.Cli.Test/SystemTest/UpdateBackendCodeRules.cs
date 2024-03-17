using System.Diagnostics;
using AspNetCore.Simple.Sdk.Mediator;
using Extensions.Pack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RunJit.Cli.Test.Commands;
using RunJit.Cli.Test.Extensions;

namespace RunJit.Cli.Test.SystemTest
{
    [TestCategory("runjit update backend coderules")]
    [TestClass]
    public class UpdateBackendCodeRulesTest : GlobalSetup
    {
        private const string BasePath = "api/update";

        [TestMethod]
        public async Task Should_Update_All_CodeRules_For_Target_Solution()
        {
            // 1. Create new Web Api
            var solutionFile = await Mediator.SendAsync(new CreateNewSimpleWebApi("RunJit.Update.CodeRule", WebApiFolder, BasePath)).ConfigureAwait(false);

            // 3. Test if generated results is buildable
            await DotNetTool.AssertRunAsync("dotnet", $"build {solutionFile.FullName}");

            // 3. Update to .Net 8
            await Mediator.SendAsync(new UpdateBackendCodeRulesForSolution(solutionFile.FullName)).ConfigureAwait(false);
        }
    }

    internal sealed record UpdateBackendCodeRulesForSolution(string solution) : ICommand;

    internal sealed class UpdateBackendCodeRulesForSolutionHandler : ICommandHandler<UpdateBackendCodeRulesForSolution>
    {
        public async Task Handle(UpdateBackendCodeRulesForSolution request, CancellationToken cancellationToken)
        {
            await using var sw = new StringWriter();
            Console.SetOut(sw);

            var strings = CollectConsoleParameters(request).ToArray();
            var consoleCall = strings.Flatten(" ");
            Console.WriteLine();
            Console.WriteLine(consoleCall);
            Debug.WriteLine(consoleCall);
            var exitCode = await Program.Main(strings);
            var output = sw.ToString();

            Assert.AreEqual(0, exitCode, output);
        }

        private IEnumerable<string> CollectConsoleParameters(UpdateBackendCodeRulesForSolution parameters)
        {
            // 1. Parameter solution file from the backend to parse
            yield return "runjit";
            yield return "update";
            yield return "backend";
            yield return "coderules";
            yield return "--solution";
            yield return parameters.solution;
        }
    }


    internal sealed record UpdateBackendCodeRulesPackagesForGitRepos(string GitRepos, string WorkingDirectory) : ICommand;

    internal sealed class UpdateBackendCodeRulesPackagesForGitReposHandler : ICommandHandler<UpdateBackendCodeRulesPackagesForGitRepos>
    {
        public async Task Handle(UpdateBackendCodeRulesPackagesForGitRepos request, CancellationToken cancellationToken)
        {
            await using var sw = new StringWriter();
            Console.SetOut(sw);

            var strings = CollectConsoleParameters(request).ToArray();
            var consoleCall = strings.Flatten(" ");
            Console.WriteLine();
            Console.WriteLine(consoleCall);
            Debug.WriteLine(consoleCall);
            var exitCode = await Program.Main(strings);
            var output = sw.ToString();

            Assert.AreEqual(0, exitCode, output);
        }

        private IEnumerable<string> CollectConsoleParameters(UpdateBackendCodeRulesPackagesForGitRepos parameters)
        {
            // 1. Parameter solution file from the backend to parse
            yield return "runjit";
            yield return "update";
            yield return "backend";
            yield return "coderules";
            yield return "--git-repos";
            yield return parameters.GitRepos;
            yield return "--working-directory";
            yield return parameters.WorkingDirectory;
        }
    }
}
