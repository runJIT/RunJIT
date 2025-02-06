using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;
using Solution.Parser.Solution;

namespace RunJit.Cli.New.MinimalApiProject.CodeGen
{
    internal static class AddRepoLinterCodeGenExtension
    {
        internal static void AddRepoLinterCodeGen(this IServiceCollection services)
        {
            services.AddConsoleService();
            services.AddNamespaceProvider();

            services.AddSingletonIfNotExists<IMinimalApiProjectRootLevelCodeGen, RepoLinterCodeGen>();
        }
    }

    internal sealed class RepoLinterCodeGen(ConsoleService consoleService) : IMinimalApiProjectRootLevelCodeGen
    {
        private const string Template = """
                                        {
                                          "extends": "https://raw.githubusercontent.com/todogroup/repolinter/master/rulesets/default.json",
                                          "version": 2,
                                          "axioms": {},
                                          "rules": {
                                            "binaries-not-present": {
                                              "level": "off",
                                              "rule": {
                                                "type": ["**/*.dll"]
                                              }
                                            }
                                          }
                                        }
                                        """;


        public async Task GenerateAsync(SolutionFile solutionFile,
                                        MinimalApiProjectInfos minimalApiProjectInfos)
        {
            // 1 Setup file name
            var file = Path.Combine(solutionFile.SolutionFileInfo.Value.Directory!.FullName, "repolinter.json");


            await File.WriteAllTextAsync(file, Template).ConfigureAwait(false);

            // 3. Print success message
            consoleService.WriteSuccess($"Successfully created {file}");
        }
    }
}
