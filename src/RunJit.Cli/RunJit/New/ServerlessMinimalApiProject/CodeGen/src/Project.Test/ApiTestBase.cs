using AspNetCore.Simple.MsTest.Sdk;
using DotNetTool.Service;
using Extensions.Pack;

namespace $ProjectName$.Test
{
    /// <summary>
    ///     Provides a base class for API tests, including setup and teardown logic for the test environment.
    /// </summary>
    [TestClass]
    public abstract class ApiTestBase
    {
        /// <summary>
        ///     The name of the Docker container used for DynamoDB during tests.
        /// </summary>
        private const string DynamoDbContainerName = "dynamodb-local";

        /// <summary>
        ///     The base class for API testing, providing utilities for setting up and interacting with the API.
        /// </summary>
        private static ApiTestBase<Program> _apiTestBase = null!;

        /// <summary>
        ///     The tool used for executing .NET commands, such as managing Docker containers.
        /// </summary>
        private static readonly IDotNetTool DotNetTool = DotNetToolFactory.Create();

        [AssemblyInitialize]
        public static async Task AssemblyInitializeAsync(TestContext _)
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
            _apiTestBase = new ApiTestBase<Program>("Development", // The environment name
                                                    (_, _) =>
                                                    {
                                                    }, // The register services action
                                                    environmentVariables); // Configure environment variables  

            // 4. We need once the http client to communicate with the started api
            Client = _apiTestBase.CreateClient();
        }

        protected static HttpClient Client { get; private set; } = null!;

        [AssemblyCleanup]
        public static async Task AssemblyCleanupAsync()
        {
            // 1. Dispose of the API test environment.
            if (_apiTestBase.IsNotNull())
            {
                await _apiTestBase.DisposeAsync().ConfigureAwait(false);
            }
            
            // 2. Dispose the http client
            Client.Dispose();
            
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
