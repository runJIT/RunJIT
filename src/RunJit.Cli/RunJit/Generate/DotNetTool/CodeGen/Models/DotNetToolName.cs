using Argument.Check;

namespace RunJit.Cli.RunJit.Generate.DotNetTool.CodeGen.Models
{
    public class DotNetToolName
    {
        public DotNetToolName(string name, string normalizedName)
        {
            Throw.IfNullOrWhiteSpace(name);

            // hint for new json method !
            // Throw.IfNullOrWhiteSpace(() => normalizedName);

            Name = name;
            NormalizedName = normalizedName;
        }

        public string Name { get; }

        public string NormalizedName { get; }
    }
}
