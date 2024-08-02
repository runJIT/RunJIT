using System.Text.RegularExpressions;

namespace RunJit.Cli.RunJit.Generate.Client;

internal static partial class GlobalRegex
{
    [GeneratedRegex("([V])\\d")]
    internal static partial Regex VersionRegex();

    [GeneratedRegex(@"<([^>]*)>")]
    internal static partial Regex GetGenericTypeRegex();

    [GeneratedRegex(@"\.MapToApiVersion\((?<version>\d+)\)")]
    internal static partial Regex MapToApiVersionRegex();

    [GeneratedRegex(@"\.HasApiVersion\(new ApiVersion\((?<version>[^\)]+)\)\)")]
    internal static partial Regex HasVersionRegex();
}
