using System.Collections.Immutable;
using Argument.Check;
using Solution.Parser.CSharp;
using Solution.Parser.Solution;

namespace RunJit.Cli.CodeRules
{
    [TestClass]
    public abstract class MsTestBase
    {
        protected static IImmutableList<CSharpSyntaxTree> ProductiveCodeSyntaxTreesToAnaylze { get; private set; } =
            ImmutableList<CSharpSyntaxTree>.Empty;

        protected static IImmutableList<CSharpSyntaxTree> TestCodeSyntaxTrees { get; private set; } =
            ImmutableList<CSharpSyntaxTree>.Empty;

        protected static IImmutableList<CSharpSyntaxTree> ProductiveCodeSyntaxTrees { get; private set; } =
            ImmutableList<CSharpSyntaxTree>.Empty;

        protected static IImmutableList<CSharpSyntaxTree> AllSyntaxTrees { get; private set; } =
            ImmutableList<CSharpSyntaxTree>.Empty;

        [AssemblyInitialize]
        public static void Init(TestContext _)
        {
            var sSolutionFileInfo = new SolutionFileName("RunJit.Cli.sln").FindSolutionFileReverseFrom(new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory));
            Throw.IfNull(sSolutionFileInfo);

            var parsedSolution = sSolutionFileInfo.Parse();

            ProductiveCodeSyntaxTrees = parsedSolution.ProductiveProjects
                                                      .Where(p => p.ProjectFileInfo.FileNameWithoutExtenion == "AspNetCore.Simple.ClientGenerator")
                                                      .SelectMany(p => p.CSharpFileInfos)
                                                      .Select(c => c.Parse())
                                                      .ToImmutableList();

            TestCodeSyntaxTrees = parsedSolution.UnitTestProjects.Where(p => p.ProjectFileInfo.FileNameWithoutExtenion == "AspNetCore.Simple.ClientGenerator.Tests")
                                                .SelectMany(p => p.CSharpFileInfos)
                                                .Select(c => c.Parse())
                                                .ToImmutableList();

            AllSyntaxTrees = parsedSolution.Projects.SelectMany(p => p.CSharpFileInfos)
                                           .Select(c => c.Parse())
                                           .ToImmutableList();
        }
    }

    public record SqlScript(FileInfo FileInfo,
                            string Content);
}
