using System.Collections.Immutable;
using Solution.Parser.AspNet;
using Solution.Parser.Project;

namespace RunJit.Cli.RunJit.Generate.CustomEndpoint
{
    public record EndpointData
    {
        public IImmutableList<Template> Templates { get; init; } = ImmutableList<Template>.Empty;
    }

    public record EndpointAction
    {
        public required string HttpAction { get; init; }

        public required string Url { get; set; }

        public string DomainActionName { get; init; } = string.Empty;

        public required IImmutableList<string> QueryParameters { get; init; } = ImmutableList<string>.Empty;

        public required IImmutableList<string> AllowedResourceTypes { get; init; } = ImmutableList<string>.Empty;
    }

    public record Template
    {
        public required string Folder { get; init; }

        public IImmutableList<CodeFile> Files { get; init; } = ImmutableList<CodeFile>.Empty;

        public IImmutableList<Template> Templates { get; init; } = ImmutableList<Template>.Empty;
    }

    public record CodeFile
    {
        public required string Name { get; set; }

        public required string Content { get; set; }
    }

    internal record UrlParameter(string Name,
                                 string Type);

    internal record QueryParameter(string Name,
                                   string Type);
}
