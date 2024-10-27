using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;
using Solution.Parser.CSharp;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    public static class AddResponseTypeHandleStrategyCodeGenExtension
    {
        public static void AddResponseTypeHandleStrategyCodeGen(this IServiceCollection services)
        {
            services.AddConsoleService();
            services.AddNamespaceProvider();

            services.AddSingletonIfNotExists<INetToolCodeGen, ResponseTypeHandleStrategyCodeGen>();
        }
    }

    internal sealed class ResponseTypeHandleStrategyCodeGen(ConsoleService consoleService,
                                                     NamespaceProvider namespaceProvider) : INetToolCodeGen
    {
        private const string Template = """
                                        using System.Collections.Immutable;
                                        using Extensions.Pack;
                                        
                                        namespace $namespace$
                                        {
                                            internal static class AddResponseTypeHandleStrategyExtension
                                            {
                                                internal static void AddResponseTypeHandleStrategy(this IServiceCollection services)
                                                {
                                                    services.AddNotOkResponseTypeHandler();
                                                    services.AddJsonResponseTypeHandler();
                                                    services.AddFileStreamResponseTypeHandler();
                                                    services.AddByteArrayResponseTypeHandler();
                                        
                                                    services.AddSingletonIfNotExists<ResponseTypeHandleStrategy>();
                                                }
                                            }
                                        
                                            internal interface ISpecificResponseTypeHandler
                                            {
                                                bool CanHandle<TResult>(HttpResponseMessage responseMessage);
                                        
                                                Task<TResult> HandleAsync<TResult>(HttpResponseMessage responseMessage,
                                                                                   HttpMethod httpMethod,
                                                                                   HttpClient httpClient,
                                                                                   string url);
                                            }
                                        
                                            internal sealed class ResponseTypeHandleStrategy
                                            {
                                                private readonly IEnumerable<ISpecificResponseTypeHandler> _responseHandlers;
                                        
                                                public ResponseTypeHandleStrategy(IEnumerable<ISpecificResponseTypeHandler> responseHandlers)
                                                {
                                                    _responseHandlers = responseHandlers;
                                                }
                                        
                                                public Task<TResult> HandleAsync<TResult>(HttpResponseMessage responseMessage,
                                                                                          HttpMethod httpMethod,
                                                                                          HttpClient httpClient,
                                                                                          string url)
                                                {
                                                    var responseHandlers = _responseHandlers.Where(handler => handler.CanHandle<TResult>(responseMessage)).ToImmutableList();
                                                    if (responseHandlers.IsEmpty())
                                                    {
                                                        throw new ProblemDetailsException("No response handler was found to handle expected response",
                                                            $"No response handler was found to handle expected response type: '{typeof(TResult).Name}'",
                                                            ("Response type", typeof(TResult).Name));
                                                    }
                                        
                                                    if (responseHandlers.Count > 1)
                                                    {
                                                        throw new ProblemDetailsException("More than one reponse handlers was found for expected response type",
                                                            $"For response type: '{typeof(TResult).Name}' {responseHandlers.Count} response handler was found",
                                                            ("Response type", typeof(TResult).Name),
                                                            ("ResponseHandlers", responseHandlers.Select(handler => handler.GetType().Name).ToImmutableList()));
                                                    }
                                        
                                                    var result = responseHandlers[0].HandleAsync<TResult>(responseMessage, httpMethod, httpClient, url);
                                                    return result;
                                                }
                                            }
                                        }
                                        """;

        public async Task GenerateAsync(FileInfo projectFileInfo,
                                        DotNetToolInfos dotNetTool)
        {
            // 1. Add ResponseTypeHandleStrategy Folder
            var appFolder = new DirectoryInfo(Path.Combine(projectFileInfo.Directory!.FullName, "ResponseTypeHandling"));

            if (appFolder.NotExists())
            {
                appFolder.Create();
            }

            // 2. Add ResponseTypeHandleStrategy.cs
            var file = Path.Combine(appFolder.FullName, "ResponseTypeHandleStrategy.cs");

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
