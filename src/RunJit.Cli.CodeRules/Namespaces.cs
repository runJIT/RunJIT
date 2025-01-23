using System.Collections.Immutable;
using ConsoleTables;
using Extensions.Pack;

namespace RunJit.Cli.CodeRules
{
    [TestCategory("Namespaces")]
    [TestCategory("Coding-Rules")]
    [TestClass]
    public class Namespaces : MsTestBase
    {
        /// <summary>
        ///     Namespaces:
        ///     - API.Comments.V2.OpenCommentsTranslated
        ///     - API.Comments.V3.Export
        ///     - API.MailTemplate.V1.Controllers
        ///     Problem: We want to detect all technical namespaces.
        ///     Solutions:
        ///     - 1. We try to split by . but too different constellation what is coming after the Version -> failed because of
        ///     different namespace constellations
        ///     - 2. We just simple check a list of technical aspects
        /// </summary>
        [TestMethod]
        public void Api_Namespaces_Should_Not_Contain_Any_Technical_Aspects()
        {
            var technicalAspects = new[]
                                   {
                                       "Facade", "Builder", "Controller",
                                       "Extensions", "Model", "Queries",
                                       "Query", "Command", "Service",
                                       "Helper", "Collector", "AppSetting",
                                       "Provider", "Sql", "Validation",
                                       "Mapper", "Wrapper"
                                   };

            var versions = new[] { "V1" };

            var invalidNamespaces = (from syntaxTree in ProductiveCodeSyntaxTreesToAnaylze
                                     let @namespace = syntaxTree.NameSpace.Name
                                     where versions.Any(v => @namespace.Contains(v, StringComparison.Ordinal))
                                     let indexOfVersion = @namespace.IndexOf(".V", StringComparison.Ordinal)
                                     let sinceVersion = @namespace.Substring(indexOfVersion, @namespace.Length - indexOfVersion - 1)
                                     where technicalAspects.Any(sinceVersion.Contains)
                                     select new
                                            {
                                                CurrentNamespace = @namespace,
                                                ExpectedNamesapce = @namespace.Replace($".{@namespace.Split('.').Last()}", string.Empty)
                                            }).ToImmutableList();

            var onlyUniqueNamespaces = invalidNamespaces.Distinct(name => name.CurrentNamespace);

            Assert.IsTrue(invalidNamespaces.IsEmpty(),
                          @$"Your namespace contains technical aspects like {technicalAspects.Flatten(", ")} or many more. 
                      Your namespace should only contain your domain and version.{Environment.NewLine}{ConsoleTable.From(onlyUniqueNamespaces)}");
        }
    }
}
