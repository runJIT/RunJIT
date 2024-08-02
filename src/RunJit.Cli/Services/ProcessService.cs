using System.CommandLine.Invocation;
using System.Text;
using Argument.Check;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Models;

namespace RunJit.Cli
{
    internal static class AddProcessServiceExtension
    {
        internal static void AddProcessService(this IServiceCollection services)
        {
            services.AddConsoleService();

            services.AddSingletonIfNotExists<IProcessService, ProcessService>();
        }
    }

    internal interface IProcessService
    {
        Task<CliRunResult> RunAsync(string command,
                                    string arguments);

        Task<CliRunResult> StartAsync(string command,
                                      string arguments);
    }

    internal class ProcessService : IProcessService
    {
        private readonly IConsoleService _consoleService;

        public ProcessService(IConsoleService consoleService)
        {
            Throw.IfNull(consoleService);

            _consoleService = consoleService;
        }

        public async Task<CliRunResult> RunAsync(string command,
                                                 string arguments)
        {
            _consoleService.WriteInfo($"{command} {arguments}");

            var stringBuilder = new StringBuilder();

            var result = await Process.ExecuteAsync(command, arguments, null,
                                                    s => stringBuilder.AppendLine(s)).ConfigureAwait(false);

            var output = stringBuilder.ToString();

            if (result.NotEqualsTo(0))
            {
                _consoleService.WriteError(output);
            }
            else
            {
                _consoleService.WriteSuccess(output);
            }

            return new CliRunResult(result, output);
        }

        public Task<CliRunResult> StartAsync(string command,
                                             string arguments)
        {
            _consoleService.WriteInfo($"{command} {arguments}");

            Process.StartProcess(command, arguments, null,
                                 s => _consoleService.WriteInfo(s), s => _consoleService.WriteError(s));

            // Here we start only the process we do not await till process will completed.
            return Task.FromResult(new CliRunResult(0, string.Empty));
        }
    }
}
