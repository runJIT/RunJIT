using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;
using Solution.Parser.CSharp;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    public static class AddHttpCallHandlerCodeGenExtension
    {
        public static void AddHttpCallHandlerCodeGen(this IServiceCollection services)
        {
            services.AddConsoleService();
            services.AddNamespaceProvider();

            services.AddHttpClient();

            services.AddSingletonIfNotExists<INetToolCodeGen, HttpCallHandlerCodeGen>();
        }
    }

    internal sealed class HttpCallHandlerCodeGen(ConsoleService consoleService,
                                          NamespaceProvider namespaceProvider) : INetToolCodeGen
    {
        private const string Template = """
                                        using System.Runtime.CompilerServices;
                                        using Microsoft.Extensions.DependencyInjection;

                                        namespace $namespace$
                                        {
                                            internal static class AddHttpCallHandlerExtension
                                            {
                                                internal static void AddHttpCallHandler(this IServiceCollection services)
                                                {
                                                    services.AddResponseTypeHandleStrategy();
                                                    services.AddHttpRequestMessageBuilder();
                                                    services.AddHttpCallHandlerFactory();
                                        
                                                    services.AddHttpClient();
                                                }
                                            }
                                        
                                            internal interface IHttpCallHandler
                                            {
                                                Task CallAsync(HttpMethod httpMethod,
                                                               string url,
                                                               object? payload,
                                                               CancellationToken cancellationToken,
                                                               [CallerArgumentExpression(nameof(payload))] string payloadParameterName = "");
                                        
                                                Task<TResult> CallAsync<TResult>(HttpMethod httpMethod,
                                                                                 string url,
                                                                                 object? payload,
                                                                                 CancellationToken cancellationToken,
                                                                                 [CallerArgumentExpression(nameof(payload))] string payloadParameterName = "");
                                            }
                                        
                                            internal sealed class HttpCallHandler : IHttpCallHandler
                                            {
                                                private readonly HttpClient _httpClient;
                                                private readonly HttpRequestMessageBuilder _httpRequestMessageBuilder;
                                                private readonly ResponseTypeHandleStrategy _responseHandleStrategy;
                                        
                                                public HttpCallHandler(HttpClient httpClient,
                                                                       ResponseTypeHandleStrategy responseHandleStrategy,
                                                                       HttpRequestMessageBuilder httpRequestMessageBuilder)
                                                {
                                                    _httpClient = httpClient;
                                                    _responseHandleStrategy = responseHandleStrategy;
                                                    _httpRequestMessageBuilder = httpRequestMessageBuilder;
                                                }
                                        
                                        
                                                public async Task CallAsync(HttpMethod httpMethod,
                                                                            string url,
                                                                            object? payload,
                                                                            CancellationToken cancellationToken,
                                                                            [CallerArgumentExpression(nameof(payload))] string payloadParameterName = "")
                                                {
                                                    var message = _httpRequestMessageBuilder.BuildFrom(httpMethod, url, payload, payloadParameterName);
                                                    var response = await _httpClient.SendAsync(message, cancellationToken).ConfigureAwait(false);
                                        
                                                    if (response.IsSuccessStatusCode)
                                                    {
                                                        return;
                                                    }
                                        
                                                    var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                                                    var absoluteUrl = $"{_httpClient.BaseAddress}{url}";
                                        
                                                    throw new ProblemDetailsException("Client call to endpoint was not successful",
                                                                                            $"The http call: {httpMethod.Method} {url} was not successful",
                                                                                            ("HttpMethod", httpMethod.Method),
                                                                                            ("Url", absoluteUrl),
                                                                                            ("Error", content));
                                                }
                                        
                                                public async Task<TResult> CallAsync<TResult>(HttpMethod httpMethod,
                                                                                              string url,
                                                                                              object? payload,
                                                                                              CancellationToken cancellationToken,
                                                                                              [CallerArgumentExpression(nameof(payload))] string payloadParameterName = "")
                                                {
                                                    var message = _httpRequestMessageBuilder.BuildFrom(httpMethod, url, payload, payloadParameterName);
                                                    var response = await _httpClient.SendAsync(message, cancellationToken).ConfigureAwait(false);
                                                    var result = await _responseHandleStrategy.HandleAsync<TResult>(response, httpMethod, _httpClient, url).ConfigureAwait(false);
                                                    return result;
                                                }
                                        
                                        
                                                public async Task CallAsync(Func<HttpClient, string, string, Task<HttpResponseMessage>> httpFunction,
                                                                            HttpMethod httpMethod,
                                                                            string url,
                                                                            string paylod)
                                                {
                                                    var response = await httpFunction(_httpClient, url, paylod);
                                                    if (response.IsSuccessStatusCode)
                                                    {
                                                        return;
                                                    }
                                        
                                                    var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                                                    var absoluteUrl = $"{_httpClient.BaseAddress}{url}";
                                                    throw new ProblemDetailsException("Client call to endpoint was not successfull",
                                                                                            $"The http call: {httpMethod.Method} {url} was not successful",
                                                                                            ("HttpMethod", httpMethod.Method),
                                                                                            ("Url", absoluteUrl),
                                                                                            ("Error", content));
                                                }
                                        
                                                public async Task<TResult> CallAsync<TResult>(Func<HttpClient, string, string, Task<HttpResponseMessage>> httpFunction,
                                                                                              HttpMethod httpMethod,
                                                                                              string url,
                                                                                              string paylod)
                                                {
                                                    var response = await httpFunction(_httpClient, url, paylod);
                                                    var result = await _responseHandleStrategy.HandleAsync<TResult>(response, httpMethod, _httpClient, url).ConfigureAwait(false);
                                                    return result;
                                                }
                                            }
                                        }

                                        """;

        public async Task GenerateAsync(FileInfo projectFileInfo,
                                        DotNetToolInfos dotNetTool)
        {
            // 1. Add HttpCallHandler Folder
            var appFolder = new DirectoryInfo(Path.Combine(projectFileInfo.Directory!.FullName, "HttpCallHandlers"));

            if (appFolder.NotExists())
            {
                appFolder.Create();
            }

            // 2. Add HttpCallHandler.cs
            var file = Path.Combine(appFolder.FullName, "HttpCallHandler.cs");

            var newTemplate = Template.Replace("$namespace$", dotNetTool.ProjectName)
                                      .Replace("$dotNetToolName$", dotNetTool.DotNetToolName.NormalizedName);

            var formattedTemplate = newTemplate.FormatSyntaxTree();

            await File.WriteAllTextAsync(file, formattedTemplate).ConfigureAwait(false);

            // 3. Adjust namespace provider
            namespaceProvider.SetNamespaceProviderAsync(projectFileInfo, $"{dotNetTool.ProjectName}.HttpCallHandlers", true);

            // 4. Print success message
            consoleService.WriteSuccess($"Successfully created {file}");
        }
    }
}
