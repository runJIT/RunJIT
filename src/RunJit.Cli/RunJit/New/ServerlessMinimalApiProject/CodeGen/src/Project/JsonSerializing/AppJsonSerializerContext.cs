using System.Text.Json.Serialization;
using Amazon.Lambda.APIGatewayEvents;

namespace $ProjectName$.JsonSerializing
{
    // https://docs.aws.amazon.com/lambda/latest/dg/dotnet-native-aot.html
    [JsonSerializable(typeof(APIGatewayProxyResponse))]
    [JsonSerializable(typeof(APIGatewayProxyRequest))]
    internal sealed partial class AppJsonSerializerContext : JsonSerializerContext
    {
    }
}
