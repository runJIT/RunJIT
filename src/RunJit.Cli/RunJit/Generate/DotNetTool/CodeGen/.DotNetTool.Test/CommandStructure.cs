using System.Xml.Linq;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.RunJit.Generate.CustomEndpoint;
using RunJit.Cli.RunJit.Generate.DotNetTool.Models;
using RunJit.Cli.Services;

namespace RunJit.Cli.RunJit.Generate.DotNetTool.DotNetTool.Test
{
    internal static class AddCommandStructureCodeGenExtension
    {
        internal static void AddCommandStructureCodeGen(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<IDotNetToolTestSpecificCodeGen, CommandStructureCodeGen>();
        }
    }

    internal sealed class CommandStructureCodeGen(ConsoleService consoleService) : IDotNetToolTestSpecificCodeGen
    {
        private const string Template = """
                                        namespace $namespace$
                                        {
                                            [TestClass]
                                            [TestCategory("$dotNetToolName$")]
                                            [TestCategory("myapi config get")]
                                            public class $commandName$Test : GlobalSetup
                                            {
                                                [TestMethod]
                                                public Task $testMethodName$()
                                                {
                                                    return Cli.AssertRunAsync("$cliCall$",
                                                                              "$expectedOutput$");
                                                }
                                            }
                                        }
                                        """;

        public async Task GenerateAsync(FileInfo projectFileInfo,
                                        XDocument projectDocument,
                                        DotNetToolInfos dotNetToolInfos)
        {
            await CreateTestsForCommandAsync(projectFileInfo, dotNetToolInfos, dotNetToolInfos.CommandInfo, null, string.Empty);

            consoleService.WriteSuccess($"Successfully created cli test structure");

            static async Task CreateTestsForCommandAsync(FileInfo projectFileInfo,
                                                         DotNetToolInfos dotNetToolInfos,
                                                         CommandInfo commandInfo,
                                                         DirectoryInfo? parentDirectory,
                                                         string cliCallPath)
            {
                // Create folder
                var folderPath = parentDirectory.IsNull() ?
                                     Path.Combine(projectFileInfo.Directory!.FullName, dotNetToolInfos.CommandInfo.NormalizedName) :
                                     Path.Combine(parentDirectory.FullName, commandInfo.NormalizedName);

                var targetFolder = new DirectoryInfo(folderPath);

                if (targetFolder.NotExists())
                {
                    targetFolder.Create();
                }

                cliCallPath = $"{cliCallPath} {commandInfo.NormalizedName}".TrimStart(' ');

                // If we have an endpoint we create test cases
                if (commandInfo.EndpointInfo.IsNotNull())
                {
                    var expectedOutput = cliCallPath.Split(" ").Where(value => value.StartsWith("-").IsFalse()).Flatten(".");
                    expectedOutput = $"{expectedOutput}.Output.{commandInfo.NormalizedName}.json";

                    var testMethodName = cliCallPath.Split(" ").Where(value => value.StartsWith("-").IsFalse()).Flatten("_");
                    var parametersFile = cliCallPath.Split(" ").Where(value => value.StartsWith("-").IsFalse()).Flatten(".");
                    parametersFile = $"{parametersFile}.Parameters.{commandInfo.NormalizedName}.json";

                    var cliCall = cliCallPath;
                    var cliCallWithArgument = commandInfo.EndpointInfo.IsNull() ? $"{cliCall}" : $"{cliCall} {parametersFile}";

                    var newTemplate = Template.Replace("$namespace$", $"{dotNetToolInfos.ProjectName}.Test")
                                                   .Replace("$cliCall$", cliCallWithArgument.ToLower())
                                                   .Replace("$testMethodName$", testMethodName)
                                                   .Replace("$expectedOutput$", expectedOutput)
                                                   .Replace("$dotNetToolName$", dotNetToolInfos.NormalizedName.ToLower())
                                                   .Replace("$commandName$", commandInfo.NormalizedName)
                                                   .Replace("$parametersFile$", parametersFile);

                    await File.WriteAllTextAsync(Path.Combine(targetFolder.FullName, $"{commandInfo.NormalizedName}Test.cs"), newTemplate).ConfigureAwait(false);

                    // Output folder
                    var outputFolder = new DirectoryInfo(Path.Combine(targetFolder.FullName, "Output"));
                    if (outputFolder.NotExists())
                    {
                        outputFolder.Create();
                    }

                    await File.WriteAllTextAsync(Path.Combine(outputFolder.FullName, $"{commandInfo.NormalizedName}.json"), "{}").ConfigureAwait(false);

                    if (commandInfo.EndpointInfo.RequestType.IsNotNull())
                    {
                        // Parameters folder
                        var parametersFolder = new DirectoryInfo(Path.Combine(targetFolder.FullName, "Parameters"));
                        if (parametersFolder.NotExists())
                        {
                            parametersFolder.Create();
                        }

                        await File.WriteAllTextAsync(Path.Combine(parametersFolder.FullName, $"{commandInfo.NormalizedName}.json"), "{}").ConfigureAwait(false);
                    }
                }

                foreach (var commandInfoSubCommand in commandInfo.SubCommands)
                {
                    await CreateTestsForCommandAsync(projectFileInfo, dotNetToolInfos, commandInfoSubCommand, targetFolder, cliCallPath);
                }
            }
        }
    }
}
