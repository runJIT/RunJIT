using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;
using Solution.Parser.CSharp;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    public static class AddByteArrayResponseTypeHandlerCodeGenExtension
    {
        public static void AddByteArrayResponseTypeHandlerCodeGen(this IServiceCollection services)
        {
            services.AddConsoleService();
            services.AddNamespaceProvider();

            services.AddSingletonIfNotExists<INetToolCodeGen, ByteArrayResponseTypeHandlerCodeGen>();
        }
    }

    internal class ByteArrayResponseTypeHandlerCodeGen(ConsoleService consoleService,
                              NamespaceProvider namespaceProvider) : INetToolCodeGen
    {
        private const string Template = """
                                        using Extensions.Pack;
                                        
                                        namespace $namespace$
                                        {
                                            internal static class AddByteArrayResponseTypeHandlerExtension
                                            {
                                                internal static void AddByteArrayResponseTypeHandler(this IServiceCollection services)
                                                {
                                                    services.AddSingletonIfNotExists<ISpecificResponseTypeHandler, ByteArrayResponseTypeHandler>();
                                                }
                                            }
                                        
                                            internal sealed class ByteArrayResponseTypeHandler : ISpecificResponseTypeHandler
                                            {
                                                public bool CanHandle<TResult>(HttpResponseMessage responseMessage)
                                                {
                                                    return responseMessage.IsSuccessStatusCode &&
                                                           typeof(TResult) == typeof(byte[]);
                                                }
                                        
                                                public async Task<TResult> HandleAsync<TResult>(HttpResponseMessage responseMessage,
                                                                                                HttpMethod httpMethod,
                                                                                                HttpClient httpClient,
                                                                                                string url)
                                                {
                                                    // Safety first this method can be called without CanHandle check !
                                                    CanHandle<TResult>(responseMessage);
                                        
                                                    var result = await responseMessage.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                                                    return (TResult)result.Cast<object>();
                                                }
                                            }
                                        }
                                        
                                        """;

        public async Task GenerateAsync(FileInfo projectFileInfo,
                                        DotNetToolInfos dotNetTool)
        {
            // 1. Add ByteArrayResponseTypeHandler Folder
            var appFolder = new DirectoryInfo(Path.Combine(projectFileInfo.Directory!.FullName, "ByteArrayResponseTypeHandler"));

            if (appFolder.NotExists())
            {
                appFolder.Create();
            }

            // 2. Add ByteArrayResponseTypeHandler.cs
            var file = Path.Combine(appFolder.FullName, "ByteArrayResponseTypeHandler.cs");

            var newTemplate = Template.Replace("$namespace$", dotNetTool.ProjectName)
                                      .Replace("$dotNetToolName$", dotNetTool.DotNetToolName.NormalizedName);

            var formattedTemplate = newTemplate.FormatSyntaxTree();

            await File.WriteAllTextAsync(file, formattedTemplate).ConfigureAwait(false);


            // 3. Adjust namespace provider
            namespaceProvider.SetNamespaceProviderAsync(projectFileInfo, $"{dotNetTool.ProjectName}.ResponseTypeHandling", true);

            // 4. Print success message
            consoleService.WriteSuccess($"Successfully created {file}");
        }
    }
}
