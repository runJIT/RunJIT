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
                                                public static async InitAsync(TestContext testContext)
                                                {
                                                    // 1. When we are in debug mode (DEV local) we start the needed docker container
                                                    //    This is a POC because we dont know yet how the end solution looks like
                                                    //    ToDo: Tear down of docker before start, to go sure container is clean and new
                                                    //          https://confluence.siemens.cloud/gms/display/PC/4.115.2+Test.Containers
                                                    //          https://jira.siemens.cloud/gms/browse/PCD-7733
                                                    if (typeof(ApiTestBase).Assembly.IsCompiledInDebug())
                                                    {
                                                        var dockerRunResult = await DotNetTool.RunAsync("docker", $"run -d -p 8001:8000 --name {DynamoDbContainerName} amazon/{DynamoDbContainerName} -jar DynamoDBLocal.jar -sharedDb").ConfigureAwait(false);
                                                        Assert.AreEqual(0, dockerRunResult.ExitCode, $"Dynamo DB: {DynamoDbContainerName} could not be started. Please check if you have docker installed");
                                                    }
                                                    
                                                
                                                    // 2. Setup and load environment variables
                                                    var environmentVariables = EmbeddedFile.GetFileContentFrom("Properties.EnvironmentVariables.json")
                                                                                           .FromJsonStringAs<Dictionary<string, string>>()
                                                                                           .Select(keyValue => (keyValue.Key, keyValue.Value)).ToArray();
                                        
                                                    
                                                    // 3. Setup api test base environment
                                                    _apiTestBase = new ApiTestBase<$webApiProjectName$.Program>("Development", // The environment name
                                                                                                        (_, _) =>
                                                                                                        {
                                                                                                        }, // The register services action
                                                                                                        environmentVariables); // Configure environment variables  
                                        
                                                    // 4. We need once the http client to communicate with the started api
                                                    _httpClient = _apiTestBase.CreateClient();
                                        
                                                    // 5. Create an instance of the DotNetTool.
                                                    var dotnetTool = DotNetToolFactory.Create();
                                        
                                                    // 6. Create a new service collection for dependency injection.
                                                    var serviceCollection = new ServiceCollection();
                                        
                                                    // 7. Register the test context and DotNetTool as singletons.
                                                    serviceCollection.AddSingleton(testContext);
                                                    serviceCollection.AddSingleton<IDotNetTool>(dotnetTool);
                                        
                                                    // 8. Register the application starter extension.
                                                    serviceCollection.AddCliRunner();
                                        
                                                    // 9. Build the service provider from the service collection.
                                                    var serviceProvider = serviceCollection.BuildServiceProvider();
                                        
                                                    // 10. Assign the service provider and CLI runner to the static properties.
                                                    Services = serviceProvider;
                                                    Cli = new CliRunner(_httpClient);
                                                }
                                        
                                                [AssemblyCleanup]
                                                public static async Task AssemblyCleanupAsync()
                                                {
                                                    // 1. Dispose the api test environment
                                                    await _apiTestBase.DisposeAsync().ConfigureAwait(false);
                                                    
                                                    // 2. Dispose the http client
                                                    _httpClient.Dispose();
                                                    
                                                    // 3. When we are in debug mode (DEV local) we start the needed docker container
                                                    //    and we have to shut down and remove it ! to have a clean container next startup
                                                    if (typeof(ApiTestBase).Assembly.IsCompiledInDebug())
                                                    {
                                                        await DotNetTool.RunAsync("docker", $"stop {DynamoDbContainerName}").ConfigureAwait(false);
                                                        await DotNetTool.RunAsync("docker", $"rm {DynamoDbContainerName}").ConfigureAwait(false);
                                                    }
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
