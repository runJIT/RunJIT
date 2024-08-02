using System.Diagnostics;
using AspNetCore.Simple.Sdk.Mediator;
using Extensions.Pack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RunJit.Cli.Test.Commands;
using RunJit.Cli.Test.Extensions;

namespace RunJit.Cli.Test.SystemTest
{
    [TestCategory("runjit fix serviceregistrations")]
    [TestClass]
    public class FixEmbeddedResourceTest : GlobalSetup
    {
        private const string Resource = "User";

        private const string BasePath = "api/client-gen";

        [TestMethod]

        //
        public async Task Fix_All_Service_Registrations()
        {
            // 1. Create new Web Api
            var solutionFile = await Mediator.SendAsync(new CreateNewSimpleWebApi("RunJit.Fix.ServiceRegistrations", WebApiFolder, BasePath)).ConfigureAwait(false);

            // 2. Create Web-Api endpoints
            await Mediator.SendAsync(new CreateSimpleRestController(solutionFile, Resource, false)).ConfigureAwait(false);

            // 3. Generate client for endpoint
            await Mediator.SendAsync(new FixEmbeddedResourceLocally(solutionFile.FullName)).ConfigureAwait(false);

            //// 4. Test if generated results is buildable
            await DotNetTool.AssertRunAsync("dotnet", $"build {solutionFile.FullName}").ConfigureAwait(false);
        }
    }

    internal sealed record FixEmbeddedResource(string GitRepos,
                                               string WorkingDirectory) : ICommand;

    internal sealed class FixEmbeddedResourceHandler : ICommandHandler<FixEmbeddedResource>
    {
        public async Task Handle(FixEmbeddedResource request,
                                 CancellationToken cancellationToken)
        {
            await using var sw = new StringWriter();
            Console.SetOut(sw);

            var strings = CollectConsoleParameters(request).ToArray();
            var consoleCall = strings.Flatten(" ");
            Console.WriteLine();
            Console.WriteLine(consoleCall);
            Debug.WriteLine(consoleCall);
            var exitCode = await Program.Main(strings).ConfigureAwait(false);
            var output = sw.ToString();

            Assert.AreEqual(0, exitCode, output);
        }

        private IEnumerable<string> CollectConsoleParameters(FixEmbeddedResource parameters)
        {
            // 1. Parameter solution file from the backend to parse
            yield return "runjit";
            yield return "fix";
            yield return "embeddedresource";
            yield return "--git-repos";
            yield return parameters.GitRepos;
            yield return "--working-directory";
            yield return parameters.WorkingDirectory;
        }
    }

    internal sealed record FixEmbeddedResourceLocally(string Solution) : ICommand;

    internal sealed class FixEmbeddedResourceLocallyHandler : ICommandHandler<FixEmbeddedResourceLocally>
    {
        public async Task Handle(FixEmbeddedResourceLocally request,
                                 CancellationToken cancellationToken)
        {
            await using var sw = new StringWriter();
            Console.SetOut(sw);

            var strings = CollectConsoleParameters(request).ToArray();
            var consoleCall = strings.Flatten(" ");
            Console.WriteLine();
            Console.WriteLine(consoleCall);
            Debug.WriteLine(consoleCall);
            var exitCode = await Program.Main(strings).ConfigureAwait(false);
            var output = sw.ToString();

            Assert.AreEqual(0, exitCode, output);
        }

        private IEnumerable<string> CollectConsoleParameters(FixEmbeddedResourceLocally parameters)
        {
            // 1. Parameter solution file from the backend to parse
            yield return "runjit";
            yield return "fix";
            yield return "embeddedresource";
            yield return "--solution";
            yield return parameters.Solution;
        }
    }
}
