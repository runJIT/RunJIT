using System.Diagnostics;
using AspNetCore.Simple.Sdk.Mediator;
using DotNetTool.Service;
using Extensions.Pack;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RunJit.Cli.Test.Commands
{
    internal sealed record CreateNewSimpleWebApi(string ProjectName,
                                                 DirectoryInfo Output,
                                                 string BasePath) : ICommand<FileInfo>;

    internal sealed class CreateNewSimpleWebApiHandler(IDotNetTool dotNetTool) : ICommandHandler<CreateNewSimpleWebApi, FileInfo>
    {
        public async Task<FileInfo> Handle(CreateNewSimpleWebApi request,
                                           CancellationToken cancellationToken)
        {
            await using var sw = new StringWriter();
            Console.SetOut(sw);

            var strings = CollectConsoleParameters(request).ToArray();
            var consoleCall = strings.Flatten(" ");
            Console.WriteLine();
            Console.WriteLine(consoleCall);
            Debug.WriteLine(consoleCall);

            var result = await dotNetTool.RunAsync("pulse", consoleCall).ConfigureAwait(false);

            var output = sw.ToString();

            Assert.AreEqual(0, result.ExitCode, result.Output);

            var projectDirectory = new DirectoryInfo(Path.Combine(request.Output.FullName, request.ProjectName));
            var solutionFile = projectDirectory.EnumerateFiles("*.sln", SearchOption.TopDirectoryOnly).First();

            return solutionFile;
        }

        private IEnumerable<string> CollectConsoleParameters(CreateNewSimpleWebApi parameters)
        {
            // 1. Parameter solution file from the backend to parseW
            yield return "new";
            yield return "simple-webapi";
            yield return "--project-name";
            yield return parameters.ProjectName;
            yield return "--base-path";
            yield return parameters.BasePath;
            yield return "--output";
            yield return parameters.Output.FullName;
        }
    }
}
