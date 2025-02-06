using System.Xml.Linq;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.New.MinimalApiProject.CodeGen.MinimalApiProject;
using RunJit.Cli.New.MinimalApiProject.CodeGen.MinimalApiProject.Test;
using Solution.Parser.Solution;

namespace RunJit.Cli.New.MinimalApiProject.CodeGen
{
    internal interface IMinimalApiProjectTestCaseCodeGen
    {
        Task<string> GenerateAsync(MinimalApiProjectInfos minimalApiProjectInfos,
                                   string cliCallPath);
    }

    internal interface IMinimalApiProjectRootLevelCodeGen
    {
        Task GenerateAsync(SolutionFile solutionFile,
                           MinimalApiProjectInfos minimalApiProjectInfos);
    }

    internal interface IMinimalApiProjectSpecificCodeGen
    {
        Task GenerateAsync(FileInfo projectFileInfo,
                           XDocument projectDocument,
                           MinimalApiProjectInfos minimalApiProjectInfos);
    }

    internal interface IMinimalApiProjectTestSpecificCodeGen
    {
        Task GenerateAsync(FileInfo projectFileInfo,
                           XDocument projectDocument,
                           MinimalApiProjectInfos minimalApiProjectInfos);
    }

    internal static class AddMinimalApiProjectCodeGenExtension
    {
        internal static void AddMinimalApiProjectCodeGen(this IServiceCollection services)
        {
            services.AddMinimalApiProjectGenerator();
            services.AddMinimalApiProjectTestGenerator();

            services.AddChangelogCodeGen();
            services.AddCodeOfConduct();
            services.AddCommitLintConfigCodeGen();
            services.AddContributingCodeGen();
            services.AddDirectoryBuildPropsCodeGen();
            services.AddDotSettingsCodeGen();
            services.AddEditorConfigCodeGen();
            services.AddGitIgnoreCodeGen();
            services.AddGlobalJsonCodeGen();
            services.AddLicenseCodeGen();
            services.AddMaintainersCodeGen();
            services.AddProjectCreationCheckListCodeGen();
            services.AddReadmeCodeGen();
            services.AddRepoLinterCodeGen();
            services.AddSecurityCodeGen();
            services.AddSupportCodeGen();

            services.AddSingletonIfNotExists<MinimalApiProjectCodeGen>();
        }
    }

    internal class MinimalApiProjectCodeGen(MinimalApiProjectGenerator minimalApiProjectGenerator,
                                            MinimalApiProjectTestGenerator minimalApiProjectTestGenerator,
                                            IEnumerable<IMinimalApiProjectRootLevelCodeGen> rootLevelCodeGens)
    {
        internal async Task GenerateAsync(SolutionFile solutionFile,
                                          NewMinimalApiProjectParameters minimalApiProjectParameters,
                                          MinimalApiProjectInfos minimalApiProjectInfos)
        {

            // 1. Write all root level code
            foreach (var minimalApiProjectRootLevelCodeGen in rootLevelCodeGens)
            {
                await minimalApiProjectRootLevelCodeGen.GenerateAsync(solutionFile, minimalApiProjectInfos).ConfigureAwait(false);
            }

            // 2. Create the new web api project    
            var webApiProjectFileInfo = await minimalApiProjectGenerator.GenerateAsync(solutionFile, minimalApiProjectInfos).ConfigureAwait(false);

            // 3. Create the test project for the new web api project    
            await minimalApiProjectTestGenerator.GenerateAsync(solutionFile, webApiProjectFileInfo, minimalApiProjectInfos).ConfigureAwait(false);
        }
    }
}
