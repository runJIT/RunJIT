using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Generate.Client
{
    internal static class AddCollectSwaggerPathExtension
    {
        internal static void AddCollectSwaggerPath(this IServiceCollection services)
        {
            services.AddCollectSwaggerPathValidator();
            services.AddCollectTillInputCorrect();

            services.AddSingletonIfNotExists<CollectSwaggerPath>();
        }
    }

    internal class CollectSwaggerPath(CollectSwaggerPathValidator inputValidator,
                                      ICollectTillInputCorrect collectTillInputCorrect)
    {
        private const string Title = @"Please enter your swagger location. Can be local file or uri. 
Sample local file: D:\Swagger\swagger.json or folder D:\Swagger\
Sample uri:        https://localhost:5001/api/sustainability/swagger/v1.0/swagger.json

Important: If you choose local file and you have multiple versions of your API's give us an folder which
           contains all swagger.json for each version. When using uri, then we can detect all api versions
           and fetch all needed data auomatically.";

        public string Collect()
        {
            var pathToSwagger = collectTillInputCorrect.CollectTillInputIsValid(Title, inputValidator);

            return pathToSwagger;
        }
    }
}
