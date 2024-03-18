namespace RunJit.Cli.CodeRules
{
    [Ignore]
    [TestCategory("Coding-Rules")]
    [TestCategory("Solution Projects")]
    [TestClass]
    public class SolutionAndProjects : MsTestBase
    {
        [TestMethod]
        public void Solution_And_Projects_Must_Not_Contains_Module_As_Middle_Name()
        {
        }
    }

    [Ignore]
    [TestCategory("Coding-Rules")]
    [TestCategory("Docker")]
    [TestClass]
    public class Docker : MsTestBase
    {
        [TestMethod]
        public void Docker_File_Must_Have_Correct_Configuration_For_Solution()
        {
        }
    }
}
