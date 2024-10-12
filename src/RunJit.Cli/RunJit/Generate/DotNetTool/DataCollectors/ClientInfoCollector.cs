using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Models;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal static class AddDotNetToolInfoCollectorExtension
    {
        internal static void AddDotNetToolInfoCollector(this IServiceCollection services)
        {
            services.AddCollectSolutionPath();
            services.AddCollectTargetPath();
            services.AddCollectSwaggerPath();

            services.AddSingletonIfNotExists<IDotNetToolInfoCollector, DotNetToolInfoCollector>();
        }
    }

    internal interface IDotNetToolInfoCollector
    {
        DotNetTool Collect(DotNetToolParameters clientGenParameters);
    }

    // ToDo: DotNetToolGen Info collector
    internal class DotNetToolInfoCollector(CollectSolutionPath collectSolutionPath) : IDotNetToolInfoCollector
    {
        public DotNetTool Collect(DotNetToolParameters clientGenParameters)
        {
            var solutionFileExists = clientGenParameters.SolutionFile.IsNotNull() && clientGenParameters.SolutionFile.Exists;

            // 1. Request backend solution
            var solutionFileInfo = solutionFileExists ? clientGenParameters.SolutionFile : collectSolutionPath.Collect();

            // If integrate into source solution we enter the solution file :) 
            var targetDirectory = solutionFileInfo;

            var projectName = $"{solutionFileInfo.NameWithoutExtension()}.DotNetTool";
            var dotnetToolName = new DotNetToolName("dotnet-clientgen", "clientgen");

            return new DotNetTool(projectName, dotnetToolName, solutionFileInfo, targetDirectory);
        }
    }
}
