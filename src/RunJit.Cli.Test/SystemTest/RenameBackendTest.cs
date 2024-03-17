using System.Diagnostics;
using AspNetCore.Simple.Sdk.Mediator;
using Extensions.Pack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RunJit.Cli.Test.Commands;
using RunJit.Cli.Test.Extensions;

namespace RunJit.Cli.Test.SystemTest
{
    [TestCategory("runjit rename backend")]
    [TestClass]
    public class RenameBackendTest : GlobalSetup
    {
        private const string Resource = "User";
        private const string BasePath = "api/new-web-api";
        private const string OldName = "RunJit.Api.To.Rename.OldName";
        private const string NewName = "RunJit.Api.To.Rename.NewName";


        [TestMethod]
        public async Task Should_Create_A_Module_And_Rename_The_Whole_Backend_From_Old_To_New_Name()
        {
            // 1. Create new Web Api
            var solutionFile = await Mediator.SendAsync(new CreateNewSimpleWebApi(OldName, WebApiFolder, BasePath)).ConfigureAwait(false);

            // 2. Create a simple web api endpoint
            await Mediator.SendAsync(new CreateSimpleRestController(solutionFile, Resource, false)).ConfigureAwait(false);

            // 3. To proof all should be fine we build the solution
            await DotNetTool.AssertRunAsync("dotnet", $"build {solutionFile.FullName}");

            // 4. Rename anything
            var renamedSolution = await Mediator.SendAsync(new RenameBackend(solutionFile.FullName, OldName, NewName)).ConfigureAwait(false);

            // 5. After renaming all should be fine if we try to build the solution
            await DotNetTool.AssertRunAsync("dotnet", $"build {renamedSolution.FullName}");
        }
    }

    internal sealed record RenameBackend(string SolutionFileOrFolder,
                                         string OldName,
                                         string NewName) : ICommand<FileInfo>;

    internal sealed class RenameBackendHandler(TestContext testContext) : ICommandHandler<RenameBackend, FileInfo>
    {
        public async Task<FileInfo> Handle(RenameBackend request, CancellationToken cancellationToken)
        {
            await using var sw = new StringWriter();
            Console.SetOut(sw);

            var strings = CollectConsoleParameters(request).ToArray();
            var consoleCall = strings.Flatten(" ");
            testContext.WriteLine(consoleCall);
            Console.WriteLine();
            Console.WriteLine(consoleCall);
            Debug.WriteLine(consoleCall);
            var exitCode = await Program.Main(strings);
            var output = sw.ToString();

            Assert.AreEqual(0, exitCode, output);

            var solutionFile = new FileInfo(request.SolutionFileOrFolder.Replace(request.OldName, request.NewName));
            return solutionFile;
        }

        private IEnumerable<string> CollectConsoleParameters(RenameBackend parameters)
        {
            // 1. Parameter solution file from the backend to parse
            yield return "runjit";
            yield return "rename";
            yield return "backend";
            yield return parameters.SolutionFileOrFolder;
            yield return "--old-name";
            yield return parameters.OldName;
            yield return "--new-name";
            yield return parameters.NewName;
        }
    }
}
