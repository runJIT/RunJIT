using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli
{
    internal static class AddEnumerationTypesExtension
    {
        internal static void AddEnumerationTypes(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<IEnumerationTypes, EnumerationTypes>();
        }
    }

    public interface IEnumerationTypes
    {
        bool IsListType(string typeName);
    }

    public class EnumerationTypes : IEnumerationTypes
    {
        // The following table shows the keywords for built-in C# types, which are aliases of predefined types in the System namespace
        // All this types will bot be found as type in the system namespace so we need this information too.
        // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/built-in-types-table
        private static readonly List<string> BaseEnumerableTypeNames = new List<string>
        {
            "Enumerable",
            "List",
            "Collection",
            "[]",
            "Array",
            "Immutable",
            "Dictionary"
        };

        public bool IsListType(string typeName)
        {
            return BaseEnumerableTypeNames.Any(type => typeName.Contains(type));
        }
    }
}
