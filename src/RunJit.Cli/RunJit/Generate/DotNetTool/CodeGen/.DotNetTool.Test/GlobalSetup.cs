using System.Xml.Linq;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;
using Solution.Parser.CSharp;
using Solution.Parser.Project;

namespace RunJit.Cli.Generate.DotNetTool.DotNetTool.Test
{
    internal static class AddGlobalSetupCodeGenExtension
    {
        internal static void AddGlobalSetupCodeGen(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<IDotNetToolTestSpecificCodeGen, GlobalSetupCodeGen>();
        }
    }

    internal sealed class GlobalSetupCodeGen(ConsoleService consoleService) : IDotNetToolTestSpecificCodeGen
    {
        private const string Template = """
                                        using AspNetCore.Simple.MsTest.Sdk;
                                        using DotNetTool.Service;
                                        using Extensions.Pack;
                                        using Microsoft.Extensions.DependencyInjection;
                                        
                                        namespace $namespace$
                                        {
                                            [TestClass]
                                            public class GlobalSetup
                                            {
                                                private static HttpClient? _httpClient;
                                        
                                                /// <summary>
                                                ///     Provides access to the service provider for dependency injection.
                                                /// </summary>
                                                protected static IServiceProvider Services { get; private set; } = null!;
                                        
                                                /// <summary>
                                                ///     Provides access to the CLI runner for executing CLI commands.
                                                /// </summary>
                                                protected static CliRunner Cli { get; private set; } = null!;
                                        
                                                private static ApiTestBase<$webApiProjectName$.Program> _apiTestBase = null!;
                                        
                                                /// <summary>
                                                ///     Initializes the test environment by setting up the service provider and CLI runner.
                                                ///     This method is called once before any tests are run.
                                                /// </summary>
                                                /// <param name="testContext">The context of the test run.</param>
                                                [AssemblyInitialize]
                                                public static void InitAsync(TestContext testContext)
                                                {
                                                    // 1. Start the web api needed for testing
                                                    var environmentVariables = EmbeddedFile.GetFileContentFrom("Properties.EnvironmentVariables.json")
                                                                                           .FromJsonStringAs<Dictionary<string, string>>()
                                                                                           .Select(keyValue => (keyValue.Key, keyValue.Value)).ToArray();
                                        
                                                    _apiTestBase = new ApiTestBase<$webApiProjectName$.Program>("Development", // The environment name
                                                                                                        (_, _) =>
                                                                                                        {
                                                                                                        }, // The register services action
                                                                                                        environmentVariables); // Configure environment variables  
                                        
                                                    _httpClient = _apiTestBase.CreateClient();
                                        
                                        
                                        
                                                    // 1. Create an instance of the DotNetTool.
                                                    var dotnetTool = DotNetToolFactory.Create();
                                        
                                                    // 2. Create a new service collection for dependency injection.
                                                    var serviceCollection = new ServiceCollection();
                                        
                                                    // 3. Register the test context and DotNetTool as singletons.
                                                    serviceCollection.AddSingleton(testContext);
                                                    serviceCollection.AddSingleton<IDotNetTool>(dotnetTool);
                                        
                                                    // 4. Register the application starter extension.
                                                    serviceCollection.AddCliRunner();
                                        
                                                    // 5. Build the service provider from the service collection.
                                                    var serviceProvider = serviceCollection.BuildServiceProvider();
                                        
                                                    // 6. Assign the service provider and CLI runner to the static properties.
                                                    Services = serviceProvider;
                                                    Cli = new CliRunner(_httpClient);
                                                }
                                        
                                                [AssemblyCleanup]
                                                public static void AssemblyCleanup()
                                                {
                                                    _apiTestBase.Dispose();
                                                    _httpClient?.Dispose();
                                                }
                                            }
                                        }
                                        
                                        """;

        public async Task GenerateAsync(FileInfo projectFileInfo,
                                        XDocument projectDocument,
                                        DotNetToolInfos dotNetToolInfos,
                                        ProjectFile? webApiProject)
        {
            // 1. GlobalSetup
            var file = Path.Combine(projectFileInfo.Directory!.FullName, "GlobalSetup.cs");

            var newTemplate = Template.Replace("$namespace$", $"{dotNetToolInfos.ProjectName}.Test")
                                      .Replace("$dotNetToolName$", dotNetToolInfos.NormalizedName)
                                      .Replace("$webApiProjectName$", webApiProject?.ProjectFileInfo.FileNameWithoutExtenion);

            var formattedTemplate = newTemplate.FormatSyntaxTree();

            await File.WriteAllTextAsync(file, formattedTemplate).ConfigureAwait(false);

            // 2. Print success message
            consoleService.WriteSuccess($"Successfully created {file}");
        }
    }
}
