using System.Diagnostics;
using AspNetCore.Simple.Sdk.Mediator;
using DotNetTool.Service;
using Extensions.Pack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RunJit.Cli.Test.Commands;

namespace RunJit.Cli.Test.SystemTest
{
    [TestCategory("runjit zip")]
    [TestClass]
    public class ZipTest : GlobalSetup
    {
        private const string ProjectName = "RunJit.Zip";
        private const string Resource = "User";
        private const string BasePath = "api/data-management";

        [TestMethod]
        public async Task Should_Be_Able_To_Zip_A_Target_Folder_Into_A_Zip_File()
        {
            // 1. Create new Web Api
            var solutionFile = await Mediator.SendAsync(new CreateNewSimpleWebApi(ProjectName, WebApiFolder, BasePath)).ConfigureAwait(false);

            // 2. Create Web-Api endpoints
            await Mediator.SendAsync(new CreateSimpleRestController(solutionFile, Resource, false)).ConfigureAwait(false);

            // 3. Zip the created Web Api
            var targetFileInfo = new FileInfo(Path.Combine(WebApiFolder.FullName, $"{ProjectName}.zip"));
            var result = await Mediator.SendAsync(new ZipDirectory(solutionFile.Directory!.FullName, targetFileInfo.FullName)).ConfigureAwait(false);

            // 4. Run tests if they exists
            Assert.IsTrue(result.Exists);
        }
    }

    internal sealed record ZipDirectory(string Directory, string ZipFile) : ICommand<FileInfo>;

    internal sealed class ZipDirectoryHandler : ICommandHandler<ZipDirectory, FileInfo>
    {
        public async Task<FileInfo> Handle(ZipDirectory request, CancellationToken cancellationToken)
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

            var zipFile = output.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Last();
            return new FileInfo(zipFile);
        }

        private IEnumerable<string> CollectConsoleParameters(ZipDirectory paramters)
        {
            // 1. Parameter solution file from the backend to parse
            yield return "runjit";
            yield return "zip";
            yield return paramters.Directory;
            yield return "--zip-file";
            yield return paramters.ZipFile;
        }
    }


    internal sealed record InstallRequiredComponents : ICommand;

    internal sealed class InstallAllInOneCliHandler(IDotNetTool donDotNetTool) : ICommandHandler<InstallRequiredComponents>
    {
        public async Task Handle(InstallRequiredComponents request, CancellationToken cancellationToken)
        {
            var result = await donDotNetTool.ExistsAsync("pulse.cli").ConfigureAwait(false);
            if (result.IsNotNull())
            {
                return;
            }
            
            var installResult = await donDotNetTool.InstallAsync("pulse", "0.1.0-alpha.372").ConfigureAwait(false);
            if (installResult.ExitCode != 0)
            {
                throw new Exception(installResult.Output);
            }
        }
    }
}
