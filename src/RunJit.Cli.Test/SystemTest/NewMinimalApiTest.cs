using System.Diagnostics;
using AspNetCore.Simple.Sdk.Mediator;
using Extensions.Pack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RunJit.Cli.Test.Extensions;

namespace RunJit.Cli.Test.SystemTest
{
    [TestCategory("runjit new minimal-api")]
    [TestClass]
    public class NewMinimalApiTest : GlobalSetup
    {
        [DataTestMethod]
        [DataRow("Siemens.Sdc", "api/core")]
        public async Task Should_Generate_New_Minimal_Web_Api_Solution(string projectName,
                                                                       string basePath)
        {
            var targetDirectory = Path.Combine(Environment.CurrentDirectory, projectName);

            // 1. Create new solution and projects
            var result = await Mediator.SendAsync(new NewMinimalApiProject(projectName, basePath, targetDirectory)).ConfigureAwait(false);

            // 2. Assert that solution can be build
            await DotNetTool.AssertRunAsync("dotnet", $"build {result.FullName}").ConfigureAwait(false);

            // 3. Assert that solution can be tested
            await DotNetTool.AssertRunAsync("dotnet", $"test {result.FullName}").ConfigureAwait(false);
        }

        internal sealed record NewMinimalApiProject(string ProjectName,
                                                    string basePath,
                                                    string TargetDirectory = "",
                                                    string ExpectedErrorMessage = "") : ICommand<FileInfo>;

        internal sealed class NewMinimalApiProjectHandler : ICommandHandler<NewMinimalApiProject, FileInfo>
        {
            public async Task<FileInfo> Handle(NewMinimalApiProject request,
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

                if (request.ExpectedErrorMessage.IsNotNullOrEmpty())
                {
                    Assert.AreEqual(1, exitCode);
                    Assert.IsTrue(output.Contains(request.ExpectedErrorMessage));
                }
                else
                {
                    Assert.AreEqual(0, exitCode, output);
                }

                // Last output must be the solution file
                var solutionFile = output.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Last();

                return new FileInfo(solutionFile);
            }

            private IEnumerable<string> CollectConsoleParameters(NewMinimalApiProject request)
            {
                yield return "runjit";
                yield return "new";
                yield return "minimal-api";
                yield return "--project-name";
                yield return request.ProjectName;
                yield return "--base-path";
                yield return request.basePath;
                yield return "--target-directory";
                yield return request.TargetDirectory;
            }
        }
    }
}
