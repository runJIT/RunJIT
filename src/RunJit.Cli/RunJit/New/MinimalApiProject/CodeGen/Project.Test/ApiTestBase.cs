using AspNetCore.Simple.MsTest.Sdk;
using Extensions.Pack;

namespace $ProjectName$.Test
{
    [TestClass]
    public abstract class ApiTestBase
    {
        private static ApiTestBase<Program> _apiTestBase = null!;

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext _)
        {
            // 1. Super simple just use the provided API test base class and you are ready to go
            var environmentVariables = EmbeddedFile.GetFileContentFrom("Properties.EnvironmentVariables.json")
                                                   .FromJsonStringAs<Dictionary<string, string>>()
                                                   .Select(keyValue => (keyValue.Key, keyValue.Value)).ToArray();
            
            _apiTestBase = new ApiTestBase<Program>("Development", // The environment name
                                                    (_, _) =>
                                                    {
                                                    }, // The register services action
                                                    environmentVariables); // Configure environment variables  

            // 2. We need once the http client to communicate with the started api
            Client = _apiTestBase.CreateClient();
        }

        protected static HttpClient Client { get; private set; } = null!;

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            _apiTestBase.Dispose();
            Client.Dispose();
        }
    }
}
