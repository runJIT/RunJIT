using System.Diagnostics;
using AspNetCore.Simple.Sdk.Mediator;
using Extensions.Pack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RunJit.Cli.Test.Commands;
using RunJit.Cli.Test.Extensions;

namespace RunJit.Cli.Test.SystemTest
{
    [TestCategory("pulse generate client")]
    [TestClass]
    public class GenerateClientTest : GlobalSetup
    {
        private const string Resource = "User";
        private const string BasePath = "api/client-gen";

        [TestMethod]
        public async Task Generate_Client_For_Simple_Rest_Endpoint()
        {
            // 1. Create new Web Api
            var solutionFile = await Mediator.SendAsync(new CreateNewSimpleWebApi("Runjit.ClientGen", WebApiFolder, BasePath)).ConfigureAwait(false);

            // 2. Create Web-Api endpoints
            await Mediator.SendAsync(new CreateSimpleRestController(solutionFile, Resource, false)).ConfigureAwait(false);

            // 3. Generate client for endpoint
            await Mediator.SendAsync(new GenerateClient(solutionFile)).ConfigureAwait(false);

            //// 4. Test if generated results is buildable
            await DotNetTool.AssertRunAsync("dotnet", $"build {solutionFile.FullName}");
        }

        // [Ignore("Dev only")]
        [DataTestMethod]
        [DataRow(@"D:\GitHub\RunJit.Api\RunJit.Api.sln")]
        public Task Generate_Client_Of_Existing_Solution_For(string solutionPath)
        {
            return Mediator.SendAsync(new GenerateClient(new FileInfo(solutionPath), false));
        }
    }

    internal sealed record GenerateClient(FileInfo SolutionFile, bool BuildBeforeGenerate = true) : ICommand;

    internal sealed class GenerateClientHandler : ICommandHandler<GenerateClient>
    {
        public async Task Handle(GenerateClient request, CancellationToken cancellationToken)
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

        private IEnumerable<string> CollectConsoleParameters(GenerateClient request)
        {
            // 1. Parameter solution file from the backend to parse
            yield return "runjit";
            yield return "generate";
            yield return "client";
            yield return "--solution";
            yield return request.SolutionFile.FullName;
            if (request.BuildBeforeGenerate)
            {
                yield return "--build";
            }
        }
    }
}
