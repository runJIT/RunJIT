using System.Diagnostics;
using AspNetCore.Simple.Sdk.Mediator;
using Extensions.Pack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RunJit.Cli.Test.Commands;
using RunJit.Cli.Test.Extensions;

namespace RunJit.Cli.Test.SystemTest
{
    [TestCategory("runjit update backend swagger tests")]
    [TestClass]
    public class UpdateBackendSwaggerTests : GlobalSetup
    {
        private const string BasePath = "api/update";

        [TestMethod]
        // 
        public async Task Should_Update_And_Create_Swagger_Tests()
        {
            // 1. Create new Web Api
            var solutionFile = await Mediator.SendAsync(new CreateNewSimpleWebApi("RunJit.Update.SwaggerTests", WebApiFolder, BasePath)).ConfigureAwait(false);

            // 3. Test if generated results is buildable
            await DotNetTool.AssertRunAsync("dotnet", $"build {solutionFile.FullName}");

            // 3. Update to .Net 8
            await Mediator.SendAsync(new UpdateBackendSwaggerTestsForGitRepos(solutionFile.FullName, WebApiFolder.FullName)).ConfigureAwait(false);
        }
    }

    internal sealed record UpdateBackendSwaggerTestsForSolution(string solution) : ICommand;

    internal sealed class UpdateBackendSwaggerTestsHandler : ICommandHandler<UpdateBackendSwaggerTestsForSolution>
    {
        public async Task Handle(UpdateBackendSwaggerTestsForSolution request, CancellationToken cancellationToken)
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

        private IEnumerable<string> CollectConsoleParameters(UpdateBackendSwaggerTestsForSolution parameters)
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


    internal sealed record UpdateBackendSwaggerTestsForGitRepos(string GitRepos, string WorkingDirectory) : ICommand;

    internal sealed class UpdateBackendSwaggerTestsForGitReposHandler : ICommandHandler<UpdateBackendSwaggerTestsForGitRepos>
    {
        public async Task Handle(UpdateBackendSwaggerTestsForGitRepos request, CancellationToken cancellationToken)
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

        private IEnumerable<string> CollectConsoleParameters(UpdateBackendSwaggerTestsForGitRepos parameters)
        {
            // 1. Parameter solution file from the backend to parse
            yield return "runjit";
            yield return "update";
            yield return "backend";
            yield return "swaggertests";
            yield return "--git-repos";
            yield return parameters.GitRepos;
            yield return "--working-directory";
            yield return parameters.WorkingDirectory;
        }
    }
}
