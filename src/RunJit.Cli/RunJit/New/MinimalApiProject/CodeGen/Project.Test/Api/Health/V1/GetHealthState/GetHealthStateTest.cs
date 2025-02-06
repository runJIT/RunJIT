namespace MinimalApi.Test.Api.Health.V1.GetHealthState
{
    [TestClass]
    [TestCategory("Health")]
    [TestCategory("Health V1")]
    public class GetHealthStateTest : ApiTestBase
    {
        [DataTestMethod]
        [DataRow("api/health")]
        [DataRow("api/v1/health")]
        public async Task GetHealthState(string route)
        {
            var healthCheckResponse = await Client.GetAsync(route).ConfigureAwait(false);

            Assert.IsTrue(healthCheckResponse.IsSuccessStatusCode);
        }
    }
}
