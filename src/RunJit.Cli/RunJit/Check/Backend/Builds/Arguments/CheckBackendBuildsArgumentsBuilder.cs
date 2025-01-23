﻿using System.CommandLine;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Check.Backend.Builds
{
    internal static class AddCheckBackendBuildsArgumentsBuilderExtension
    {
        internal static void AddCheckBackendBuildsArgumentsBuilder(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<ICheckBackendBuildsArgumentsBuilder, CheckBackendBuildsArgumentsBuilder>();
        }
    }

    internal interface ICheckBackendBuildsArgumentsBuilder
    {
        IEnumerable<System.CommandLine.Argument> Build();
    }

    internal sealed class CheckBackendBuildsArgumentsBuilder : ICheckBackendBuildsArgumentsBuilder
    {
        public IEnumerable<System.CommandLine.Argument> Build()
        {
            yield return BuildSourceOption();
        }

        public System.CommandLine.Argument BuildSourceOption()
        {
            return new Argument<string> { Name = "solutionFile" };
        }
    }
}
