using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;
using Solution.Parser.CSharp;

namespace RunJit.Cli.RunJit.Generate.DotNetTool.DotNetTool.Test
{
    internal static class AddCliRunnerCodeGenExtension
    {
        internal static void AddCliRunnerCodeGen(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<IDotNetToolTestSpecificCodeGen, CliRunnerCodeGen>();
        }
    }

    internal sealed class CliRunnerCodeGen(ConsoleService consoleService) : IDotNetToolTestSpecificCodeGen
    {
        private const string Template = """
                                        using System.Runtime.CompilerServices;
                                        using System.Text.Json.Nodes;
                                        using AspNetCore.Simple.MsTest.Sdk;
                                        using Microsoft.Extensions.DependencyInjection;
                                        using Extensions.Pack;
                                        using System.Text.RegularExpressions;
                                        
                                        namespace $namespace$
                                        {
                                            /// <summary>
                                            /// Extension method to add the application starter to the service collection.
                                            /// </summary>
                                            internal static class AddCliRunnerExtension
                                            {
                                                /// <summary>
                                                /// Adds the CliRunner as a singleton service if it does not already exist.
                                                /// </summary>
                                                /// <param name="services">The service collection to which the CliRunner is added.</param>
                                                internal static void AddCliRunner(this IServiceCollection services)
                                                {
                                                    services.AddSingletonIfNotExists<CliRunner>();
                                                }
                                            }
                                        
                                            /// <summary>
                                            /// Represents the result of a CLI run, including the output and exit code.
                                            /// </summary>
                                            internal sealed record CliRunResult(string Output, int ExitCode);
                                        
                                            /// <summary>
                                            /// Class responsible for running CLI commands and capturing their output.
                                            /// </summary>
                                            public class CliRunner
                                            {
                                                /// <summary>
                                                /// Runs the specified CLI command asynchronously and returns the result.
                                                /// </summary>
                                                /// <param name="consoleArguments">The arguments to pass to the CLI command.</param>
                                                /// <returns>A task representing the asynchronous operation, with a CliRunResult as the result.</returns>
                                                internal async Task<CliRunResult> RunAsync(string[] consoleArguments)
                                                {
                                                    // 1. Remember the original console output.
                                                    var originalConsoleOut = Console.Out;
                                                    
                                                    // 2. Create a string writer to intercept all console outs
                                                    await using var sw = new StringWriter();
                                        
                                                    // 3. Print out the original console call
                                                    //    Important before we switch the console out
                                                    //    because we won't that output in the response
                                                    var consoleCall = consoleArguments.Flatten(" ");
                                                    Console.WriteLine(consoleCall);
                                        
                                                    // 4. Set the new output to the string writer
                                                    Console.SetOut(sw);
                                        
                                                    // 5. Run the CLI command and get the exit code
                                                    var exitCode = await Program.Main(consoleArguments).ConfigureAwait(false);
                                        
                                                    // 6. Get the output from the string writer
                                                    var output = sw.ToString();
                                        
                                                    // 7. Reset the console output
                                                    Console.SetOut(originalConsoleOut);
                                        
                                                    // 8. Trim the line breaks from the output
                                                    var trimLineBreaks = output.TrimStart('\r').TrimStart('\n').TrimEnd('\n').TrimEnd('\r');
                                        
                                                    // 9. Return the result
                                                    return new CliRunResult(trimLineBreaks, exitCode);
                                                }
                                            }
                                        
                                            /// <summary>
                                            /// Extension methods for the CliRunner class to facilitate running and asserting CLI commands.
                                            /// </summary>
                                            public static partial class CliRunnerExtensions
                                            {
                                                [GeneratedRegex(@"(\S+)\s+(\S+)\s+(\S+)\s+({.*})")]
                                                private static partial Regex ConsoleArgsSplitterRegex();
                                        
                                                /// <summary>
                                                /// Runs the specified CLI command and asserts the output.
                                                /// </summary>
                                                /// <param name="starter">The CliRunner instance.</param>
                                                /// <param name="cliCall">The CLI command to run.</param>
                                                /// <returns>A task representing the asynchronous operation, with the output as the result.</returns>
                                                public static Task<string> AssertRunAsync(this CliRunner starter, string cliCall)
                                                {
                                                    return starter.AssertRunAsync(cliCall, string.Empty, string.Empty);
                                                }
                                        
                                                /// <summary>
                                                /// Runs the specified CLI command and asserts the output, returning a FileSystemInfo object.
                                                /// </summary>
                                                /// <typeparam name="T">The type of FileSystemInfo to return.</typeparam>
                                                /// <param name="starter">The CliRunner instance.</param>
                                                /// <param name="cliCall">The CLI command to run.</param>
                                                /// <returns>A task representing the asynchronous operation, with a FileSystemInfo object as the result.</returns>
                                                public static Task<T> AssertRunAsync<T>(this CliRunner starter, string cliCall) where T : FileSystemInfo
                                                {
                                                    return starter.AssertRunAsync<T>(cliCall, string.Empty, string.Empty);
                                                }
                                        
                                                /// <summary>
                                                /// Runs the specified CLI command and asserts the output, returning a FileSystemInfo object.
                                                /// </summary>
                                                /// <typeparam name="T">The type of FileSystemInfo to return.</typeparam>
                                                /// <param name="starter">The CliRunner instance.</param>
                                                /// <param name="cliCall">The CLI command to run.</param>
                                                /// <param name="expectedOutput">The expected output of the CLI command.</param>
                                                /// <param name="expectedResultName">The name of the expected result parameter.</param>
                                                /// <returns>A task representing the asynchronous operation, with a FileSystemInfo object as the result.</returns>
                                                public static async Task<T> AssertRunAsync<T>(this CliRunner starter,
                                                                                              string cliCall,
                                                                                              string expectedOutput,
                                                                                              [CallerArgumentExpression(nameof(expectedOutput))]
                                                                                          string expectedResultName = "") where T : FileSystemInfo
                                                {
                                                    var result = await starter.AssertRunAsync(cliCall, expectedOutput, expectedResultName);
                                        
                                                    var exists = Path.Exists(result);
                                        
                                                    if (exists)
                                                    {
                                                        var fileInfo = new FileInfo(result);
                                        
                                                        return fileInfo.Cast<T>();
                                                    }
                                        
                                                    throw new InvalidOperationException("The console output is not a file path. Please check the output or your call to fix or change it.");
                                                }
                                        
                                                /// <summary>
                                                /// Runs the specified CLI command and asserts the output.
                                                /// </summary>
                                                /// <param name="starter">The CliRunner instance.</param>
                                                /// <param name="cliCall">The CLI command to run.</param>
                                                /// <param name="expectedOutput">The expected output of the CLI command.</param>
                                                /// <param name="expectedResultName">The name of the expected result parameter.</param>
                                                /// <returns>A task representing the asynchronous operation, with the output as the result.</returns>
                                                public static Task<string> AssertRunAsync(this CliRunner starter,
                                                                                          string cliCall,
                                                                                          string expectedOutput,
                                                                                          [CallerArgumentExpression(nameof(expectedOutput))]
                                                                                      string expectedResultName = "")
                                                {
                                                    var parameters = cliCall.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                                        
                                                    if (cliCall.Contains('{', StringComparison.OrdinalIgnoreCase))
                                                    {
                                                        var match = ConsoleArgsSplitterRegex().Match(cliCall);
                                                        parameters = match.Groups.Values.Skip(1).Select(g => g.Value).ToArray();
                                                    }
                                        
                                                    return starter.AssertRunAsync(parameters, expectedOutput, expectedResultName);
                                                }
                                        
                                                /// <summary>
                                                /// Runs the specified CLI command and asserts the output.
                                                /// </summary>
                                                /// <param name="starter">The CliRunner instance.</param>
                                                /// <param name="consoleArguments">The arguments to pass to the CLI command.</param>
                                                /// <param name="expectedOutput">The expected output of the CLI command.</param>
                                                /// <param name="expectedResultName">The name of the expected result parameter.</param>
                                                /// <returns>A task representing the asynchronous operation, with the output as the result.</returns>
                                                public static async Task<string> AssertRunAsync(this CliRunner starter,
                                                                                                string[] consoleArguments,
                                                                                                string expectedOutput,
                                                                                                [CallerArgumentExpression(nameof(expectedOutput))]
                                                                                            string expectedResultName = "")
                                                {
                                                    var result = await starter.RunAsync(consoleArguments);
                                                    var currentOutput = result.Output;
                                        
                                                    // Check if embedded resources
                                                    if (expectedOutput.EndsWith(".txt", StringComparison.OrdinalIgnoreCase) ||
                                                        expectedOutput.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                                                    {
                                                        expectedOutput = EmbeddedFile.GetFileContentFrom(expectedOutput);
                                                    }
                                        
                                                    // Check if valid exit code
                                                    Assert.AreEqual(0, result.ExitCode, currentOutput);
                                        
                                                    // If no expected result value was passed, no further checks are needed
                                                    if (expectedOutput.IsNullOrWhiteSpace())
                                                    {
                                                        return currentOutput;
                                                    }
                                        
                                                    // If the output is a file path, read the file content
                                                    if (Path.Exists(currentOutput))
                                                    {
                                                        currentOutput = await File.ReadAllTextAsync(currentOutput);
                                                    }
                                        
                                                    // Compare JSON objects if the output contains JSON
                                                    if (currentOutput.Contains('{', StringComparison.OrdinalIgnoreCase))
                                                    {
                                                        var current = JsonNode.Parse(currentOutput);
                                                        var expected = JsonNode.Parse(expectedOutput);
                                        
                                                        Assert.That.ObjectsAreEqual(expected, current, expectedResultParameterName: expectedResultName);
                                                    }
                                                    else
                                                    {
                                                        Assert.That.ObjectsAreEqual(expectedOutput, currentOutput, expectedResultParameterName: expectedResultName);
                                                    }
                                        
                                                    return result.Output;
                                                }
                                            }
                                        }
                                        """;

        public async Task GenerateAsync(FileInfo projectFileInfo,
                                        DotNetToolInfos dotNetToolInfos)
        {
            // 1. CliRunner
            var file = Path.Combine(projectFileInfo.Directory!.FullName, "CliRunner.cs");

            var newTemplate = Template.Replace("$namespace$", $"{dotNetToolInfos.ProjectName}.Test")
                                      .Replace("$dotNetToolName$", dotNetToolInfos.NormalizedName);

            var formattedTemplate = newTemplate.FormatSyntaxTree();

            await File.WriteAllTextAsync(file, formattedTemplate).ConfigureAwait(false);

            // 2. Print success message
            consoleService.WriteSuccess($"Successfully created {file}");
        }
    }
}
