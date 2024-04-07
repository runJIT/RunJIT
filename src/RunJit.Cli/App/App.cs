using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using Argument.Check;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.ErrorHandling;
using RunJit.Cli.RunJit;

namespace RunJit.Cli
{
    /// <summary>
    ///     Entry pint of the dot net global tool (CLI)
    /// </summary>
    internal class App(IServiceProvider serviceProvider)
    {
        public async Task<int> RunAsync(string[] args)
        {
            Throw.IfNull(args);
            
            // Get needed service to invoke client generator. Important anything have to be handled by dependency injection
            var rootCommand = serviceProvider.GetService<IRunJitCommandBuilder>()!.Build();
            var errorHandler = serviceProvider.GetService<IErrorHandler>()!;
            var dotNetCliArgumentFixer = serviceProvider.GetService<IDotNetCliArgumentFixer>()!;

            // Setup command line builder from microsoft cli sdk
            var commandLineBuilder = new CommandLineBuilder(rootCommand);
            commandLineBuilder.UseMiddleware(errorHandler.HandleErrorsAsync);
            commandLineBuilder.UseDefaults();
            var parser = commandLineBuilder.Build();

            // We automatically add a version command
            var option = parser.Configuration.RootCommand.Options.Single(o => o.Name == "version").As<Option?>();
            option?.AddAlias("-v");

            // Fix or update command parameter
            var fixedArgs = dotNetCliArgumentFixer.Fix(args);

            // Here the cli sdk of microsoft will be invoked and manage any command execution
            var result = await parser.InvokeAsync(fixedArgs).ConfigureAwait(false);
            return result;
        }
    }
}
