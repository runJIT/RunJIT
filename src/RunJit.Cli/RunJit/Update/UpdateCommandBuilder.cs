﻿using System.CommandLine;
using Extensions.Pack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.RunJit.Rename.Solution;
using RunJit.Cli.RunJit.Update.BuildConfig;
using RunJit.Cli.RunJit.Update.CodeRules;
using RunJit.Cli.RunJit.Update.Net;
using RunJit.Cli.RunJit.Update.Nuget;
using RunJit.Cli.RunJit.Update.ResharperSettings;
using RunJit.Cli.RunJit.Update.SwaggerTests;

namespace RunJit.Cli.RunJit.Update
{
    public static class AddUpdateCommandBuilderExtension
    {
        public static void AddUpdateCommandBuilder(this IServiceCollection services,
                                                   IConfiguration configuration)
        {
            services.AddBackendCommandBuilder();
            services.AddUpdateCodeRulesCommandBuilder(configuration);

            services.AddDotNetCommandBuilder();
            services.AddUpdateNugetCommandBuilder();
            services.AddUpdateCodeRulesCommandBuilder(configuration);
            services.AddUpdateSwaggerTestsCommandBuilder();
            services.AddUpdateResharperSettingsCommandBuilder();
            services.AddUpdateBuildConfigCommandBuilder();

            services.AddSingletonIfNotExists<IRunJitSubCommandBuilder, UpdateCommandBuilder>();
        }
    }

    internal sealed class UpdateCommandBuilder(IEnumerable<IUpdateSubCommandBuilder> subCommandBuilders)
        : IRunJitSubCommandBuilder
    {
        public Command Build()
        {
            var command = new Command("update", "Update the ultimate RunJit.Cli");
            subCommandBuilders.ForEach(x => command.AddCommand(x.Build()));

            return command;
        }
    }
}
