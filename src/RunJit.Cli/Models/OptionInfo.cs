using System.Runtime.CompilerServices;
using Newtonsoft.Json;

[assembly: InternalsVisibleTo("DotNetTool.Builder.Test")]

namespace RunJit.Cli.Models
{
    [method: JsonConstructor]
    public class OptionInfo(
        string value,
        string name,
        string alias,
        string description,
        bool isRequired,
        ArgumentInfo? argument,
        string normalizedValue)
        : InfoBase(value, name, normalizedValue)
    {
        public string Alias { get; } = alias;

        public string Description { get; } = description;

        public bool IsIsRequired { get; } = isRequired;

        public ArgumentInfo? Argument { get; } = argument;
    }
}
