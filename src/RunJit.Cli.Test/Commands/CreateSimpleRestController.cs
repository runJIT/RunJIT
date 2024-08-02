using System.Diagnostics;
using AspNetCore.Simple.Sdk.Mediator;
using DotNetTool.Service;
using Extensions.Pack;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RunJit.Cli.Test.Commands
{
    internal sealed record CreateSimpleRestController(FileInfo Solution,
                                                      string ResourceName,
                                                      bool WithCaching) : ICommand;

    internal sealed class CreateSimpleRestControllerHandler(IDotNetTool donDotNetTool) : ICommandHandler<CreateSimpleRestController>
    {
        public async Task Handle(CreateSimpleRestController request,
                                 CancellationToken cancellationToken)
        {
            await using var sw = new StringWriter();
            Console.SetOut(sw);

            var strings = CollectConsoleParameters(request).ToArray();
            var consoleCall = strings.Flatten(" ");
            Console.WriteLine();
            Console.WriteLine(consoleCall);
            Debug.WriteLine(consoleCall);

            var result = await donDotNetTool.RunAsync("pulse", consoleCall).ConfigureAwait(false);

            Assert.AreEqual(0, result.ExitCode, result.Output);
        }

        private IEnumerable<string> CollectConsoleParameters(CreateSimpleRestController createSimpleRestController)
        {
            // 1. Parameter solution file from the backend to parse
            yield return "new";
            yield return "simple-rest-endpoint";
            yield return "--solution";
            yield return createSimpleRestController.Solution.FullName;
            yield return "--resource-Name";
            yield return createSimpleRestController.ResourceName;
            yield return "--with-caching";
            yield return createSimpleRestController.WithCaching.ToInvariantString();
        }
    }
}
