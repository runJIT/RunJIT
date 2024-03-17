using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Extensions.Pack;

namespace RunJit.Cli.CodeRules
{
    //([V])\d

    // POC => Simple demonstration to decide if we want to go that way or go for roslyn analyzers
    [TestCategory("Coding-Rules")]
    [TestCategory("Controllers")]
    [TestClass]
    public class Classes : MsTestBase
    {
        [TestMethod]
        public void Classes_Should_Not_Contain_Versions_This_Have_To_Be_Solved_By_Namespace()
        {
            var regex = new Regex("([V])\\d");

            var classNameWithVersionInfo = (from syntaxTree in ProductiveCodeSyntaxTrees
                                            from @class in syntaxTree.Classes
                                            let match = regex.Match(@class.Name)
                                            where match.Success && @class.Name.DoesNotContain("Extension") // important register extensions like AddCommentsV1 is valid with version
                                            select new
                                            {
                                                Error = $@"
Your class name contains version infos, please manage this over your namespaces !
----------------------------------------------------------------------------------------------------------------------------
FullName:     {@class.FullQualifiedName}
----------------------------------------------------------------------------------------------------------------------------
Class name:   {@class.Name}
----------------------------------------------------------------------------------------------------------------------------
Matches:      {match}
----------------------------------------------------------------------------------------------------------------------------
"
                                            }).ToImmutableList();

            Assert.IsTrue(classNameWithVersionInfo.IsEmpty(),
                $"Class names with version infos detected. Found '{classNameWithVersionInfo.Count}':{Environment.NewLine}{classNameWithVersionInfo.Select(error => error.Error).Flatten(Environment.NewLine)}{Environment.NewLine}");
        }

    }
}
