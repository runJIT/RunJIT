using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;
using Solution.Parser.CSharp;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal static class AddJsonResponseTypeHandlerCodeGenExtension
    {
        internal static void AddJsonResponseTypeHandlerCodeGen(this IServiceCollection services)
        {
            services.AddConsoleService();
            services.AddNamespaceProvider();

            services.AddSingletonIfNotExists<INetToolCodeGen, JsonResponseTypeHandlerCodeGen>();
        }
    }

    internal sealed class JsonResponseTypeHandlerCodeGen(ConsoleService consoleService,
                              NamespaceProvider namespaceProvider) : INetToolCodeGen
    {
        private const string Template = """
                                        using Extensions.Pack;
                                        using Microsoft.AspNetCore.Mvc;
                                        
                                        namespace $namespace$
                                        {
                                            internal static class AddJsonResponseTypeHandlerExtension
                                            {
                                                internal static void AddJsonResponseTypeHandler(this IServiceCollection services)
                                                {
                                                    services.AddSingletonIfNotExists<ISpecificResponseTypeHandler, JsonResponseTypeHandler>();
                                                }
                                            }
                                        
                                            internal sealed class JsonResponseTypeHandler : ISpecificResponseTypeHandler
                                            {
                                                public bool CanHandle<TResult>(HttpResponseMessage responseMessage)
                                                {
                                                    return responseMessage.IsSuccessStatusCode &&
                                                           typeof(TResult) != typeof(byte[]) &&
                                                           typeof(TResult) != typeof(FileStreamResult);
                                                }
                                        
                                                public async Task<TResult> HandleAsync<TResult>(HttpResponseMessage responseMessage,
                                                                                                HttpMethod httpMethod,
                                                                                                HttpClient httpClient,
                                                                                                string url)
                                                {
                                                    // Safety first this method can be called without CanHandle check !
                                                    if (CanHandle<TResult>(responseMessage).IsFalse())
                                                    {
                                                        throw new ProblemDetailsException("JsonResponseTypeHandler was called without checking CanHandle.",
                                                                                          $"The response type handler for type: {typeof(TResult).Name} is not supported",
                                                                                          ("SupportedTypes", "Serializable content"));
                                                    }
                                                    
                                                    var content = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                                                    var result = content.FromJsonStringAs<TResult>();
                                                    
                                                    return result;
                                                }
                                            }
                                        }
                                        
                                        """;

        public async Task GenerateAsync(FileInfo projectFileInfo,
                                        DotNetToolInfos dotNetTool)
        {
            // 1. Add JsonResponseTypeHandler Folder
            var appFolder = new DirectoryInfo(Path.Combine(projectFileInfo.Directory!.FullName, "ResponseTypeHandling"));

            if (appFolder.NotExists())
            {
                appFolder.Create();
            }

            // 2. Add JsonResponseTypeHandler.cs
            var file = Path.Combine(appFolder.FullName, "JsonResponseTypeHandler.cs");

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
