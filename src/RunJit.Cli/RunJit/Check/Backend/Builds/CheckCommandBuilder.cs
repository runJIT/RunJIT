using System.CommandLine;
using System.CommandLine.Invocation;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.RunJit.Fix.EmbededResources;

namespace RunJit.Cli.RunJit.Check.Backend.Builds
{
    public static class AddCheckBackendBuildsCommandBuilderExtension
    {
        public static void AddCheckBackendBuildsCommandBuilder(this IServiceCollection services)
        {
            services.AddCheckBackendBuildsArgumentsBuilder();
            services.AddCheckBackendBuildsOptionsBuilder();
            services.AddCheckBackendBuilds();

            services.AddSingletonIfNotExists<ICheckBackendSubCommandBuilder, CheckBackendBuildsCommandBuilder>();
        }
    }

    internal sealed class CheckBackendBuildsCommandBuilder(ICheckBackendBuildsOptionsBuilder checkBackendBuildsOptionsBuilder,
        ICheckBackendBuilds checkBackendBuilds) : ICheckBackendSubCommandBuilder
    {
        public Command Build()
        {
            var checkCommand = new Command("builds", "The command to check that all backends are buildable. Why we need it. Cause if new .Net updates comes out it could be new analyzer finds issues which do not before.");

            //checkBackendBuildsArgumentsBuilder.Build().ForEach(arg => checkCommand.AddArgument(arg));
            checkBackendBuildsOptionsBuilder.Build().ForEach(opt => checkCommand.AddOption(opt));

            checkCommand.Handler = CommandHandler.Create<string, string, string, string>((solution, gitRepos, workingDirectory, ignorePackages) => checkBackendBuilds.HandleAsync(new CheckBackendBuildsParameters(solution ?? string.Empty, gitRepos ?? string.Empty, workingDirectory ?? string.Empty, ignorePackages ?? string.Empty)));

            return checkCommand;
        }
    }
}
