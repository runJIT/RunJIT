using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.RunJit.Generate.DotNetTool.DotNetTool.Test;
using RunJit.Cli.RunJit.Generate.DotNetTool.Models;
using Solution.Parser.Solution;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal interface IDotNetToolSpecificCodeGen
    {
        Task GenerateAsync(FileInfo projectFileInfo,
                           DotNetToolInfos dotNetToolInfos);
    }

    internal interface IDotNetToolTestSpecificCodeGen
    {
        Task GenerateAsync(FileInfo projectFileInfo,
                           DotNetToolInfos dotNetToolInfos);
    }

    internal static class AddDotNetToolCodeGenExtension
    {
        internal static void AddDotNetToolCodeGen(this IServiceCollection services)
        {
            services.AddDotNetToolGenerator();
            services.AddDotNetToolTestGenerator();

            services.AddSingletonIfNotExists<DotNetToolCodeGen>();
        }
    }

    internal class DotNetToolCodeGen(DotNetToolGenerator dotNetToolGenerator,
                                     DotNetToolTestGenerator dotNetToolTestGenerator)
    {
        internal async Task GenerateAsync(SolutionFile solutionFile,
                                          DotNetToolInfos dotNetToolInfos)
        {
            // 1. Create the new .Net tool project
            var netToolProject = await dotNetToolGenerator.GenerateAsync(solutionFile, dotNetToolInfos).ConfigureAwait(false);

            // 2. Generate all needed test code for
            await dotNetToolTestGenerator.GenerateAsync(solutionFile, netToolProject, dotNetToolInfos).ConfigureAwait(false);
        }
    }
}
