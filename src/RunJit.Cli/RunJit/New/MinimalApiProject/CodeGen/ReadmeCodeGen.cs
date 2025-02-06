using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;
using Solution.Parser.Solution;

namespace RunJit.Cli.New.MinimalApiProject.CodeGen
{
    internal static class AddReadmeCodeGenExtension
    {
        internal static void AddReadmeCodeGen(this IServiceCollection services)
        {
            services.AddConsoleService();
            services.AddNamespaceProvider();

            services.AddSingletonIfNotExists<IMinimalApiProjectRootLevelCodeGen, ReadmeCodeGen>();
        }
    }

    internal sealed class ReadmeCodeGen(ConsoleService consoleService) : IMinimalApiProjectRootLevelCodeGen
    {
        private const string Template = """
                                        # Siemens.Data.Cloud.Core
                                        
                                        ### How to setup?
                                        1. Download Docker
                                        2. Install [amazon/dynamo-local](https://hub.docker.com/r/amazon/dynamodb-local)
                                        3. Run installed dynamo with this command 
                                        ```docker run -d -p 8001:8000 --name dynamodb-local amazon/dynamodb-local -jar DynamoDBLocal.jar -sharedDb```
                                            1. Please note that you can also run the dynamo without sharedDb Option, but running with gives you the possibility to check everything also in console or DataGrip to see the actual items.
                                        4. If you change the ports (feel free to do so), you can adapt the environment variables of your local startup.
                                        
                                        [LICENSE:](./siemens-data-cloud-core/LICENSE.md)
                                        """;


        public async Task GenerateAsync(SolutionFile solutionFile,
                                        MinimalApiProjectInfos minimalApiProjectInfos)
        {
            // 1 Setup file name
            var file = Path.Combine(solutionFile.SolutionFileInfo.Value.Directory!.FullName, "README.md");


            await File.WriteAllTextAsync(file, Template).ConfigureAwait(false);

            // 3. Print success message
            consoleService.WriteSuccess($"Successfully created {file}");
        }
    }
}
