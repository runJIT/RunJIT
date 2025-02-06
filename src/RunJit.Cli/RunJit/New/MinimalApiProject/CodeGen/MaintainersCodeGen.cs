using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;
using Solution.Parser.Solution;

namespace RunJit.Cli.New.MinimalApiProject.CodeGen
{
    internal static class AddMaintainersCodeGenExtension
    {
        internal static void AddMaintainersCodeGen(this IServiceCollection services)
        {
            services.AddConsoleService();
            services.AddNamespaceProvider();

            services.AddSingletonIfNotExists<IMinimalApiProjectRootLevelCodeGen, MaintainersCodeGen>();
        }
    }

    internal sealed class MaintainersCodeGen(ConsoleService consoleService) : IMinimalApiProjectRootLevelCodeGen
    {
        private const string Template = """
                                        ALL COMPONENTS
                                        M:	Philip Pregler <philip.pregler@siemens.com>
                                        M:	René Peuser <rene.peuser.ext@siemens.com>
                                        M:	Maximilian Kleinert <maximilian.kleinert@siemens.com>
                                        """;


        public async Task GenerateAsync(SolutionFile solutionFile,
                                        MinimalApiProjectInfos minimalApiProjectInfos)
        {
            // 1 Setup file name
            var file = Path.Combine(solutionFile.SolutionFileInfo.Value.Directory!.FullName, "MAINTAINERS.md");


            await File.WriteAllTextAsync(file, Template).ConfigureAwait(false);

            // 3. Print success message
            consoleService.WriteSuccess($"Successfully created {file}");
        }
    }
}
