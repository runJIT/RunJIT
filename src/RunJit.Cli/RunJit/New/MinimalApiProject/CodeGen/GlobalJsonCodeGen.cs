using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;
using Solution.Parser.Solution;

namespace RunJit.Cli.New.MinimalApiProject.CodeGen
{
    internal static class AddGlobalJsonCodeGenExtension
    {
        internal static void AddGlobalJsonCodeGen(this IServiceCollection services)
        {
            services.AddConsoleService();
            services.AddNamespaceProvider();

            services.AddSingletonIfNotExists<IMinimalApiProjectRootLevelCodeGen, GlobalJsonCodeGen>();
        }
    }

    internal sealed class GlobalJsonCodeGen(ConsoleService consoleService) : IMinimalApiProjectRootLevelCodeGen
    {
        private const string Template = """
                                        {
                                          "sdk": {
                                            "version": "9.0.100",
                                            "rollForward": "patch"
                                          }
                                        }
                                        """;


        public async Task GenerateAsync(SolutionFile solutionFile,
                                        MinimalApiProjectInfos minimalApiProjectInfos)
        {
            // 1 Setup file name
            var file = Path.Combine(solutionFile.SolutionFileInfo.Value.Directory!.FullName, "global.json");


            await File.WriteAllTextAsync(file, Template).ConfigureAwait(false);

            // 3. Print success message
            consoleService.WriteSuccess($"Successfully created {file}");
        }
    }
}
