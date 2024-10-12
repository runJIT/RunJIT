using System.CommandLine.Invocation;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;

namespace RunJit.Cli.ErrorHandling
{
    internal static class AddErrorHandlerExtension
    {
        internal static void AddErrorHandler(this IServiceCollection services)
        {
            services.AddConsoleService();

            services.AddSingletonIfNotExists<IErrorHandler, ErrorHandler>();
        }
    }

    internal interface IErrorHandler
    {
        Task HandleErrorsAsync(InvocationContext context,
                               Func<InvocationContext, Task> next);
    }

    internal class ErrorHandler(IConsoleService consoleService) : IErrorHandler
    {
        public async Task HandleErrorsAsync(InvocationContext context,
                                            Func<InvocationContext, Task> next)
        {
            try
            {
                await next(context).ConfigureAwait(false);
            }
            catch (RunJitException e)
            {
                consoleService.WriteError(e.Message);
                context.ResultCode = 1;
            }
            catch (Exception e)
            {
                consoleService.WriteError("An unhandled Error occurred:");
                consoleService.WriteLine();
                consoleService.WriteError(e.ToString());
                context.ResultCode = 1;
            }
        }
    }
}
