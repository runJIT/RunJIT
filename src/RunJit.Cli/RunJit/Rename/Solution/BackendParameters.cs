﻿using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Rename.Solution
{
    internal static class AddBackendParametersExtension
    {
        internal static void AddBackendParameters(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<BackendParameters>();
        }
    }

    internal record BackendParameters(string FileOrFolder,
                                      string OldName,
                                      string NewName);
}
