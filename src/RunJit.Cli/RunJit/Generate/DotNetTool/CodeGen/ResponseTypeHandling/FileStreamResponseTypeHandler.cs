﻿using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;
using Solution.Parser.CSharp;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    public static class AddFileStreamResponseTypeHandlerCodeGenExtension
    {
        public static void AddFileStreamResponseTypeHandlerCodeGen(this IServiceCollection services)
        {
            services.AddConsoleService();
            services.AddNamespaceProvider();

            services.AddSingletonIfNotExists<INetToolCodeGen, FileStreamResponseTypeHandlerCodeGen>();
        }
    }

    internal sealed class FileStreamResponseTypeHandlerCodeGen(ConsoleService consoleService,
                              NamespaceProvider namespaceProvider) : INetToolCodeGen
    {
        private const string Template = """
                                        using System.Net.Mime;
                                        using Extensions.Pack;
                                        using Microsoft.AspNetCore.Mvc;
                                        
                                        namespace $namespace$
                                        {
                                            internal static class AddFileStreamResponseTypeHandlerExtension
                                            {
                                                internal static void AddFileStreamResponseTypeHandler(this IServiceCollection services)
                                                {
                                                    services.AddSingletonIfNotExists<ISpecificResponseTypeHandler, FileStreamTypeHandler>();
                                                }
                                            }
                                        
                                            internal sealed class FileStreamTypeHandler : ISpecificResponseTypeHandler
                                            {
                                                public async Task<TResult> HandleAsync<TResult>(HttpResponseMessage responseMessage,
                                                                                                HttpMethod httpMethod,
                                                                                                HttpClient httpClient,
                                                                                                string url)
                                                {
                                                    // Safety first this method can be called without CanHandle check !
                                                    if (CanHandle<TResult>(responseMessage).IsFalse())
                                                    {
                                                        throw new ProblemDetailsException("FileStreamResponseTypeHandler was called without checking CanHandle.",
                                                                                          $"The response type handler for type: {typeof(TResult).Name} is not supported",
                                                                                          ("SupportedTypes", "FileStreamResult"));
                                                    }
                                                    
                                                    var content = await responseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false);
                                                    var fileName = responseMessage.Content.Headers.ContentDisposition?.FileName ?? string.Empty;
                                                    var fileStreamResult = new FileStreamResult(content, MediaTypeNames.Application.Octet) { FileDownloadName = fileName };
                                                    return fileStreamResult.Cast<TResult>();
                                                }
                                        
                                                public bool CanHandle<TResult>(HttpResponseMessage responseMessage)
                                                {
                                                    return responseMessage.IsSuccessStatusCode &&
                                                           typeof(TResult) == typeof(FileStreamResult);
                                                }
                                            }
                                        }
                                        
                                        """;

        public async Task GenerateAsync(FileInfo projectFileInfo,
                                        DotNetToolInfos dotNetTool)
        {
            // 1. Add FileStreamResponseTypeHandler Folder
            var appFolder = new DirectoryInfo(Path.Combine(projectFileInfo.Directory!.FullName, "ResponseTypeHandling"));

            if (appFolder.NotExists())
            {
                appFolder.Create();
            }

            // 2. Add FileStreamResponseTypeHandler.cs
            var file = Path.Combine(appFolder.FullName, "FileStreamResponseTypeHandler.cs");

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
