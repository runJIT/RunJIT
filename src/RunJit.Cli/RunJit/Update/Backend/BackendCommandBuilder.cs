using System.CommandLine;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.RunJit.Update.Backend.CodeRules;
using RunJit.Cli.RunJit.Update.Backend.Net;
using RunJit.Cli.RunJit.Update.Backend.Nuget;
using RunJit.Cli.RunJit.Update.Backend.ResharperSettings;
using RunJit.Cli.RunJit.Update.Backend.SwaggerTests;

namespace RunJit.Cli.RunJit.Update.Backend
{
    public static class AddBackendCommandBuilderExtension
    {
        public static void AddBackendCommandBuilder(this IServiceCollection services)
        {
            services.AddDotNetCommandBuilder();
            services.AddUpdateNugetCommandBuilder();
            services.AddUpdateCodeRulesCommandBuilder();
            services.AddUpdateSwaggerTestsCommandBuilder();
            services.AddUpdateResharperSettingsCommandBuilder();

            services.AddSingletonIfNotExists<IUpdateSubCommandBuilder, BackendCommandBuilder>();
        }
    }

    internal class BackendCommandBuilder(IEnumerable<IBackendSubCommandBuilder> subCommandBuilders) : IUpdateSubCommandBuilder
    {
        public Command Build()
        {
            var command = new Command("backend", "Commands to do some actions on backends");
            subCommandBuilders.ForEach(x => command.AddCommand(x.Build()));
            return command;
        }
    }
}
