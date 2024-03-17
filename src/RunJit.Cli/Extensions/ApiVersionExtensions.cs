using Extensions.Pack;
using Solution.Parser.AspNet;

namespace RunJit.Cli.Extensions
{
    internal static class ApiVersionExtensions
    {
        internal static ApiVersion ToApiVersion(this string version)
        {
            if (version.IsNullOrWhiteSpace())
            {
                return new ApiVersion { Major = 1 };
            }

            var splittedString = version.Split(".");
            if (int.TryParse(splittedString[0], out var major))
            {
                return new ApiVersion { Major = major };
            }

            var vSplit = version.Split(new[] { 'v', 'V' },StringSplitOptions.RemoveEmptyEntries);
            if (int.TryParse(vSplit.Last(), out var majorFromVersion))
            {
                return new ApiVersion { Major = majorFromVersion };
            }
            
            return new ApiVersion { Major = 1 };
        }

        internal static string GetVersionForCode(this ApiVersion apiVersion)
        {
            return $"V{apiVersion.Major}";
        }
    }
}
