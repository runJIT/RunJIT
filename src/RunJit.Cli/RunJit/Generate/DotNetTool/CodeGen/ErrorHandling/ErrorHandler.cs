using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    public static class AddErrorHandlerCodeGenExtension
    {
        public static void AddErrorHandlerCodeGen(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<INetToolCodeGen, ErrorHandlerCodeGen>();
        }
    }

    internal class ErrorHandlerCodeGen(IConsoleService consoleService) : INetToolCodeGen
    {
        private const string template = """
                                        using System;
                                        using System.CommandLine.Invocation;
                                        using System.Threading.Tasks;
                                        using DotNetTool.Builder.Services;
                                        using DotNetTool.Builder.ToolBuilder.FromConsole.Services;
                                        
                                        namespace DotNetTool.Builder.ErrorHandling
                                        {
                                            internal sealed class ErrorHandler(IConsoleService consoleService) : IErrorHandler
                                            {
                                                public async Task HandleErrors(InvocationContext context, Func<InvocationContext, Task> next)
                                                {
                                                    try
                                                    {
                                                        await next(context).ConfigureAwait(false);
                                                    }
                                                    catch (Exception e)
                                                    {
                                                        var ex = FindMostSuitableException(e);
                                        
                                                        if (ex is DotNetToolBuilderException)
                                                        {
                                                            consoleService.WriteError(ex.Message);
                                                        }
                                                        else
                                                        {
                                                            consoleService.WriteError("An unhandled Error occurred:");
                                                            consoleService.WriteLine();
                                                            consoleService.WriteError(ex.ToString());
                                                        }
                                        
                                                        context.ResultCode = 1;
                                                    }
                                                }
                                        
                                                private static Exception FindMostSuitableException(Exception exception)
                                                {
                                                    if (exception is DotNetToolBuilderException)
                                                    {
                                                        return exception;
                                                    }
                                        
                                                    if (exception.InnerException != null)
                                                    {
                                                        return FindMostSuitableException(exception.InnerException);
                                                    }
                                        
                                                    return exception;
                                                }
                                            }
                                        }
                                        
                                        """;

        public async Task GenerateAsync(FileInfo projectFileInfo,
                                        DotNetToolInfos dotNetTool)
        {
            // 1. Add ErrorHandling Folder
            var appFolder = new DirectoryInfo(Path.Combine(projectFileInfo.Directory!.FullName, "ErrorHandling"));
            if (appFolder.NotExists())
            {
                appFolder.Create();
            }

            // 2. Add ErrorHandler.cs
            var file = Path.Combine(appFolder.FullName, "ErrorHandler.cs");
            await File.WriteAllTextAsync(file, template).ConfigureAwait(false);

            // 3. Print success message
            consoleService.WriteSuccess($"Successfully created {file}");
        }
    }
}
