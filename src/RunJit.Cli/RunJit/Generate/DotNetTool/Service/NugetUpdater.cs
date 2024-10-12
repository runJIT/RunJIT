﻿using System.Collections.Immutable;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using NuGet.Versioning;
using Solution.Parser.Project;
using Solution.Parser.Solution;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    public static class AddNugetUpdaterExtension
    {
        public static void AddNugetUpdater(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<NugetUpdater>();
        }
    }

    internal class NugetUpdater
    {
        internal async Task UpdateAsync(SolutionFile solutionFile, ProjectFile project)
        {
            // 1. Get all nuget packages in solution
            var allNugetPackagesInSolution = solutionFile.Projects.SelectMany(p => p.PackageReferences).Select(p => new { Package = p, NugetVersion = NuGetVersion.Parse(p.PackageVersion.Value) }).ToImmutableList();


            // 2. Read csproj first, we do not want write continuously the same file, just once if needed
            var csprojContent = await File.ReadAllTextAsync(project.ProjectFileInfo.Value.FullName).ConfigureAwait(false);
            var newCsprojContent = csprojContent;

            // 3. Iterate all packages and check if an package is already used in parent solution
            foreach (var package in project.PackageReferences)
            {
                // If package not included in the client csproj go next.
                var otherPackages = allNugetPackagesInSolution.Where(p => p.Package.Include == package.Include).ToImmutableList();
                if (otherPackages.IsEmpty)
                {
                    continue;
                }

                // Find the highest version in the solution
                var packageDotNetToolNugetVersion = NuGetVersion.Parse(package.PackageVersion.Value);

                // Detect highest nuget version
                var maxVersion = otherPackages.MaxBy(p => p.NugetVersion.Version);
                if (maxVersion.IsNull())
                {
                    continue;
                }

                // if nuget version is higher as in the template we have to upgrade it.
                if (maxVersion.NugetVersion.Version > packageDotNetToolNugetVersion.Version)
                {
                    // Important unique reference is Include + Version. Do not just replace a version can go wrong
                    newCsprojContent = newCsprojContent.Replace($@"Include=""{package.Include}"" Version=""{package.PackageVersion.Value}""",
                        $@"Include=""{package.Include}"" Version=""{maxVersion.NugetVersion.OriginalVersion}""");
                }
            }

            // If we detect changes to the org csproj we update the csproj file
            if (csprojContent.NotEqualsTo(newCsprojContent))
            {
                await File.WriteAllTextAsync(project.ProjectFileInfo.Value.FullName, newCsprojContent).ConfigureAwait(false);
            }
        }
    }
}
