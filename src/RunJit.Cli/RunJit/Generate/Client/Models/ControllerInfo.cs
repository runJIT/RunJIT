using System.Collections.Immutable;
using System.Diagnostics;
using Attribute = Solution.Parser.CSharp.Attribute;

namespace RunJit.Cli.RunJit.Generate.Client
{
    [DebuggerDisplay("{Name}")]
    public record ControllerInfo
    {
        public required string Name { get; init; } = string.Empty;

        public required string DomainName { get; init; } = string.Empty;

        public required VersionInfo Version { get; init; } = new("1.0", "V1");

        public required string BaseUrl { get; init; } = string.Empty;

        public required string GroupName { get; init; } = string.Empty;

        public required IImmutableList<MethodInfos> Methods { get; init; } = ImmutableList<MethodInfos>.Empty;

        public IImmutableList<Attribute> Attributes { get; set; } = ImmutableList<Attribute>.Empty;
    }
}
