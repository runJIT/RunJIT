using Amazon.Lambda.AspNetCoreServer;

namespace $ProjectName$.Aws
{
    public class LambdaEntryPoint : APIGatewayProxyFunction
    {
        /// <summary>
        /// This method is used to configure the ASP.NET Core application.
        /// </summary>
        /// <param name="builder"></param>
        protected override void Init(IWebHostBuilder builder)
        {
            // For a typical Startup.cs configuration:
            builder.UseStartup<Program>();
        }
    }
}
