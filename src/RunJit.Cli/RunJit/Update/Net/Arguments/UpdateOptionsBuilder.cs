﻿using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Update.Net
{
    internal static class AddDotNetArgumentsBuilderExtension
    {
        internal static void AddDotNetArgumentsBuilder(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<IDotNetArgumentsBuilder, DotNetArgumentsBuilder>();
        }
    }

    internal interface IDotNetArgumentsBuilder
    {
        IEnumerable<System.CommandLine.Argument> Build();
    }

    internal sealed class DotNetArgumentsBuilder : IDotNetArgumentsBuilder
    {
        public IEnumerable<System.CommandLine.Argument> Build()
        {
            yield return BuildSourceOption();
        }

        public System.CommandLine.Argument BuildSourceOption()
        {
            return new System.CommandLine.Argument<string>()
                   {
                       Name = "solutionFile",
                   };
        }
    }
}
