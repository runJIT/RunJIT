using System.Diagnostics;
using AspNetCore.Simple.Sdk.Mediator;
using Extensions.Pack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RunJit.Cli.Test.Commands;

namespace RunJit.Cli.Test.SystemTest
{
    [TestCategory("runjit generate .nettool --from-api")]
    [TestClass]
    public class GenerateDotNetToolTest : GlobalSetup
    {
        private const string BasePath = "api/from-sql";

        [TestMethod]       
        public async Task Generate_Cli_For_Web_Api()
        {
            // 1. Create new Web Api
            var solutionFile = await Mediator.SendAsync(new CreateNewSimpleWebApi("Pulse.NetTool.FromApi", WebApiFolder, BasePath)).ConfigureAwait(false);

            // 2. Create Web-Api endpoints
            await Mediator.SendAsync(new CreateSimpleRestController(solutionFile, "User", false)).ConfigureAwait(false);

            // 3. Generate client for endpoint
            await Mediator.SendAsync(new GenerateDotNetTool(solutionFile)).ConfigureAwait(false);
        }

        [TestMethod]     
        public async Task Generate_Cli_For_Minimal_Web_Api()
        {
            //// 1. Create new Web Api
            //var solutionFile = await Mediator.SendAsync(new CreateNewSimpleWebApi("Pulse.NetTool.FromApi", WebApiFolder, BasePath)).ConfigureAwait(false);

            //// 2. Create Web-Api endpoints
            //await Mediator.SendAsync(new CreateSimpleRestController(solutionFile, "User", false)).ConfigureAwait(false);

            var solutionFile =  new FileInfo(@"D:\AzureDevOps\AspNetCore.MinimalApi.Sdk\AspNetCore.MinimalApi.Sdk.sln");

            // 3. Generate client for endpoint
            await Mediator.SendAsync(new GenerateDotNetTool(solutionFile)).ConfigureAwait(false);
        }

        [Ignore("Only DEV tests")]
        [TestMethod]
        //
        public async Task Generate_Cli_For_Web_Api_Dev_Only()
        {
            // 1. Create new Web Api
            var solutionFile = new FileInfo(@"D:\Siemens\pulse-core\PulseCore.sln");

            // 3. Generate client for endpoint
            await Mediator.SendAsync(new GenerateDotNetTool(solutionFile)).ConfigureAwait(false);
        }
    }

    internal sealed record GenerateDotNetTool(FileInfo Solution) : ICommand;

    internal sealed class GenerateDotNetToolHandler : ICommandHandler<GenerateDotNetTool>
    {
        public async Task Handle(GenerateDotNetTool request, CancellationToken cancellationToken)
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

            Debug.WriteLine(output);

            Assert.AreEqual(0, exitCode, output);
        }

        private IEnumerable<string> CollectConsoleParameters(GenerateDotNetTool request)
        {
            // 1. Parameter solution file from the backend to parse
            yield return "runjit";
            yield return "generate";
            yield return ".nettool";
            yield return "--solution";
            yield return request.Solution.FullName;
            yield return "--tool-name";
            yield return "myapi";
            yield return "--from-api";

        }
    }
}
