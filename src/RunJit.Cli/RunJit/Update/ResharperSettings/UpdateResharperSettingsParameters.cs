﻿using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Update.ResharperSettings
{
    internal static class AddUpdateResharperSettingsParametersExtension
    {
        internal static void AddUpdateResharperSettingsParameters(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<UpdateResharperSettingsParameters>();
        }
    }

    internal record UpdateResharperSettingsParameters(string SolutionFile,
                                                      string GitRepos,
                                                      string WorkingDirectory,
                                                      string IgnorePackages);
}
