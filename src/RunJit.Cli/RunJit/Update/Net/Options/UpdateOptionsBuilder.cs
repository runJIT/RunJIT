﻿using System.CommandLine;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.Update
{
    internal static class AddUpdateDotNetVersionOptionsBuilderExtension
    {
        internal static void AddUpdateDotNetVersionOptionsBuilder(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<IUpdateDotNetVersionOptionsBuilder, UpdateDotNetVersionOptionsBuilder>();
        }
    }

    internal interface IUpdateDotNetVersionOptionsBuilder
    {
        IEnumerable<Option> Build();
    }

    internal sealed class UpdateDotNetVersionOptionsBuilder : IUpdateDotNetVersionOptionsBuilder
    {
        public IEnumerable<Option> Build()
        {
            yield return GitRepos();
            yield return SolutionFile();
            yield return WorkingDirectory();
            yield return Version();
        }

        public Option GitRepos()
        {
            return new Option(new[] { "--git-repos", "-gr" }, "The git repository urls. Sample: 'codecommit::eu-central-1://runjit-dbi' or multiple 'codecommit::eu-central-1://runjit-dbi;codecommit::eu-central-1://runjit-dbi' separated by ';'")
            {
                Required = false,
                Argument = new Argument<string>("gtiRepos") { Description = "The git repository urls. Sample: 'codecommit::eu-central-1://runjit-dbi' or multiple 'codecommit::eu-central-1://runjit-dbi;codecommit::eu-central-1://runjit-dbi' separated by ';'" }
            };
        }

        public Option SolutionFile()
        {
            return new Option(new[] { "--solution", "-s" }, "The solution file which should be updated")
            {
                Required = false,
                Argument = new Argument<string>("solution") { Description = "The solution file which should be updated" }
            };
        }

        public Option WorkingDirectory()
        {
            return new Option(new[] { "--working-directory", "-wd" }, "The working directory in which all operation should be executed")
            {
                Required = false,
                Argument = new Argument<string>("workingDirectory") { Description = "The working directory in which all operation should be executed" }
            };
        }

        public Option Version()
        {
            return new Option(new[] { "--version", "-v" }, "The .Net version which should be the target one. Sample 6,7,8")
            {
                Required = false,
                Argument = new Argument<string>("version") { Description = "The .Net version which should be the target one. Sample 6,7,8" }
            };
        }
    }
}
