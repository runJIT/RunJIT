using System.Xml.Linq;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using Solution.Parser.Solution;

namespace RunJit.Cli.Services
{
    internal static class AddFindUsedNetVersionExtension
    {
        public static void AddFindUsedNetVersion(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<FindUsedNetVersion>();
        }
    }

    internal class FindUsedNetVersion
    {
        private const string DefaultNetVersion = "net8.0";

        public string GetNetVersion(SolutionFile solutionFile)
        {
            var netFrameworkInSolution = solutionFile.ProductiveProjects[0].TargetFrameworkVersion.FirstOrDefault();

            if (netFrameworkInSolution.IsNullOrEmpty())
            {
                // We have to check directory build props
                var directoryBuildProps = solutionFile.SolutionFileInfo.Value.Directory!.EnumerateFiles("Directory.Build.props").FirstOrDefault();

                if (directoryBuildProps.IsNull())
                {
                    // Default this was the min when we developed the code
                    return DefaultNetVersion;
                }

                var directoryBuildPropsAsXDocument = XDocument.Load(directoryBuildProps.FullName);

                // Find the TargetFramework or TargetFrameworks element
                var targetFramework = directoryBuildPropsAsXDocument.Descendants("TargetFramework").FirstOrDefault()?.Value;

                if (targetFramework.IsNotNullOrWhiteSpace())
                {
                    return targetFramework;
                }

                var targetFrameworks = directoryBuildPropsAsXDocument.Descendants("TargetFrameworks").FirstOrDefault()?.Value.Split(";").OrderBy(i => i).LastOrDefault();

                if (targetFrameworks.IsNotNullOrWhiteSpace())
                {
                    return targetFrameworks;
                }
            }

            return DefaultNetVersion;
        }
    }
}
