using System.Runtime.CompilerServices;
using Newtonsoft.Json;
[assembly: InternalsVisibleTo("DotNetTool.Builder.Test")]

namespace RunJit.Cli.RunJit.Generate.DotNetTool.Models
{
    [method: JsonConstructor]
    public class ArgumentInfo(string name,
                              string description,
                              string value,
                              string type,
                              string optimizedType,
                              string normalizedName)
        : InfoBase(value, name, normalizedName)
    {
        public string Description { get; } = description;

        public string Type { get; } = type;

        public string OptimizedType { get; set; } = optimizedType;
    }
}
