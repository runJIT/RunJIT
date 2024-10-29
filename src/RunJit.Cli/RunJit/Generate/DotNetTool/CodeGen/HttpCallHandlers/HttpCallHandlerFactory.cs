using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;
using Solution.Parser.CSharp;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    public static class AddHttpCallHandlerFactoryCodeGenExtension
    {
        public static void AddHttpCallHandlerFactoryCodeGen(this IServiceCollection services)
        {
            services.AddConsoleService();
            services.AddNamespaceProvider();

            services.AddSingletonIfNotExists<INetToolCodeGen, HttpCallHandlerFactoryCodeGen>();
        }
    }

    internal sealed class HttpCallHandlerFactoryCodeGen(ConsoleService consoleService,
                                                        NamespaceProvider namespaceProvider) : INetToolCodeGen
    {
        private const string Template = """
                                        using Extensions.Pack;
                                        using Microsoft.Extensions.DependencyInjection;

                                        namespace $namespace$
                                        {
                                            internal static class AddHttpCallHandlerFactoryExtension
                                            {
                                                internal static void AddHttpCallHandlerFactory(this IServiceCollection services)
                                                {
                                                    services.AddResponseTypeHandleStrategy();
                                                    services.AddHttpRequestMessageBuilder();
                                        
                                                    services.AddSingletonIfNotExists<HttpCallHandlerFactory>();
                                                }
                                            }
                                        
                                            internal sealed class HttpCallHandlerFactory(ResponseTypeHandleStrategy responseTypeHandleStrategy,
                                                                                         HttpRequestMessageBuilder httpRequestMessageBuilder)
                                            {
                                                internal HttpCallHandler CreateFrom(HttpClient client)
                                                {
                                                    return new HttpCallHandler(client, responseTypeHandleStrategy, httpRequestMessageBuilder);
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
            var file = Path.Combine(appFolder.FullName, "HttpCallHandlerFactory.cs");

            var newTemplate = Template.Replace("$namespace$", dotNetTool.ProjectName)
                                      .Replace("$dotNetToolName$", dotNetTool.DotNetToolName.NormalizedName);

            var formattedTemplate = newTemplate.FormatSyntaxTree();

            await File.WriteAllTextAsync(file, formattedTemplate).ConfigureAwait(false);

            // 3. Adjust namespace provider
            namespaceProvider.SetNamespaceProviderAsync(projectFileInfo, $"{dotNetTool.ProjectName}.HttpCallHandlerFactory", true);

            // 4. Print success message
            consoleService.WriteSuccess($"Successfully created {file}");
        }
    }
}
