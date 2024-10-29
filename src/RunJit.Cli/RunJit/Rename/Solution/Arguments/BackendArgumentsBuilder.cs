﻿using System.CommandLine;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Rename.Solution
{
    internal static class AddBackendArgumentsBuilderExtension
    {
        internal static void AddBackendArgumentsBuilder(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<IBackendArgumentsBuilder, BackendArgumentsBuilder>();
        }
    }

    internal interface IBackendArgumentsBuilder
    {
        IEnumerable<System.CommandLine.Argument> Build();
    }

    internal sealed class BackendArgumentsBuilder : IBackendArgumentsBuilder
    {
        public IEnumerable<System.CommandLine.Argument> Build()
        {
            yield return FileOrFolderArgument();
        }

        public System.CommandLine.Argument FileOrFolderArgument()
        {
            return new Argument<string>()
                   {
                       Name = "fileOrFolder",
                       Description = "The folder or solution path to your backend. Sample '.' for current directory or C:\\Users\\User\\source\\repos\\RunJit.Cli\\RunJit.Cli.sln explicit solution file"
                   };
        }
    }
}
