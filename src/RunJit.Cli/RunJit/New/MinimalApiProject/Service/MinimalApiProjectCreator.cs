﻿using System.Collections.Immutable;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.ErrorHandling;
using RunJit.Cli.Generate.DotNetTool;
using RunJit.Cli.Generate.DotNetTool.Models;
using RunJit.Cli.RunJit.Generate.Client;
using RunJit.Cli.Services;
using RunJit.Cli.Services.Endpoints;
using RunJit.Cli.Services.Net;
using RunJit.Cli.Services.Resharper;
using Solution.Parser.Solution;

namespace RunJit.Cli.New.MinimalApiProject
{
    internal static class AddMinimalApiProjectCreatorExtension
    {
        internal static void AddMinimalApiProjectCreator(this IServiceCollection services)
        {
            services.AddControllerParser();
            services.AddApiTypeLoader();
            services.AddRestructureController();
            services.AddEndpointsToCliCommandStructureConverter();
            services.AddMinimalApiEndpointParser();
            services.AddOrganizeMinimalEndpoints();
            services.AddDotNetToolCodeGen();
            services.AddSolutionCodeCleanup();

            services.AddSingletonIfNotExists<MinimalApiProjectCreator>();
        }
    }

    internal sealed class MinimalApiProjectCreator(MinimalApiProjectCodeGen minimalApiProjectCodeGen,
                                                   IDotNet dotNet)

    {
        internal async Task<FileInfo> GenerateProjectAsync(NewMinimalApiProjectParameters minimalApiProjectParameters)
        {
            // 1. Generate empty solution
            var solutionFileName = minimalApiProjectParameters.ProjectName.EndsWith(".sln") ? minimalApiProjectParameters.ProjectName : $"{minimalApiProjectParameters.ProjectName}.sln";

            // 2. Eval target directory
            if (minimalApiProjectParameters.TargetDirectoryInfo.Exists)
            {
                minimalApiProjectParameters.TargetDirectoryInfo.Delete(true);
            }

            minimalApiProjectParameters.TargetDirectoryInfo.Create();

            // 3. Create new solution
            await dotNet.RunAsync("dotnet", $"dotnet new sln --name {minimalApiProjectParameters.ProjectName} --output {minimalApiProjectParameters.TargetDirectoryInfo.FullName}").ConfigureAwait(false);

            // 2. Parse the solution
            var targetSolutionFile = new FileInfo(Path.Combine(minimalApiProjectParameters.TargetDirectoryInfo.FullName, solutionFileName));
            if (targetSolutionFile.NotExists())
            {
                throw new RunJitException($"The target solution: {targetSolutionFile.FullName} was not found please check if the Web-Api solution was successfully created and if so, check the path of the creation.");
            }

            var parsedSolution = new SolutionFileInfo(targetSolutionFile.FullName).Parse();
            var minimalApiProjectInfos = new MinimalApiProjectInfos
            {
                ProjectName = minimalApiProjectParameters.ProjectName,
                NetVersion = minimalApiProjectParameters.TargetFramework < 9 ? $"net9.0" : $"net{minimalApiProjectParameters.TargetFramework}.0",
                BasePath = minimalApiProjectParameters.BasePath,
                Name = minimalApiProjectParameters.ProjectName,
                NormalizedName = minimalApiProjectParameters.ProjectName,
            };

            // 11. Run all code generators
            await minimalApiProjectCodeGen.GenerateAsync(parsedSolution, minimalApiProjectParameters, minimalApiProjectInfos).ConfigureAwait(false);

            // 12. Cleanup code
            // await solutionCodeCleanup.CleanupSolutionAsync(targetSolutionFile).ConfigureAwait(false);

            return parsedSolution.SolutionFileInfo.Value;
        }
    }
}
