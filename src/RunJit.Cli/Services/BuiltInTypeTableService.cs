using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Models;

namespace RunJit.Cli
{
    internal static class AddBuiltInTypeTableServiceExtension
    {
        internal static void AddBuiltInTypeTableService(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<IBuiltInTypeTableService, BuiltInTypeTableService>();
        }
    }

    public interface IBuiltInTypeTableService
    {
        TypeToAlias? GetTypeFor(string typeName);
    }

    public class BuiltInTypeTableService : IBuiltInTypeTableService
    {
        // The following table shows the keywords for built-in C# types, which are aliases of predefined types in the System namespace
        // All this types will bot be found as type in the system namespace so we need this information too.
        // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/built-in-types-table
        private static readonly List<TypeToAlias> BuiltInTypeTable = new List<TypeToAlias>
        {
            new TypeToAlias(typeof(bool), "bool"),
            new TypeToAlias(typeof(byte), "byte"),
            new TypeToAlias(typeof(sbyte), "sbyte"),
            new TypeToAlias(typeof(char), "char"),
            new TypeToAlias(typeof(decimal), "decimal"),
            new TypeToAlias(typeof(double), "double"),
            new TypeToAlias(typeof(float), "float"),
            new TypeToAlias(typeof(int), "int"),
            new TypeToAlias(typeof(uint), "uint"),
            new TypeToAlias(typeof(long), "long"),
            new TypeToAlias(typeof(ulong), "ulong"),
            new TypeToAlias(typeof(object), "object"),
            new TypeToAlias(typeof(short), "short"),
            new TypeToAlias(typeof(ushort), "ushort"),
            new TypeToAlias(typeof(string), "string"),
            new TypeToAlias(typeof(Task), "Task"),
            new TypeToAlias(typeof(CancellationToken), "CancellationToken")

        };

        public TypeToAlias? GetTypeFor(string typeName)
        {
            var result = BuiltInTypeTable.FirstOrDefault(table =>
            {
                var alias = table.Alias.ToLowerInvariant();

                // now we are able to detect
                // string
                // string?
                // string[]
                return typeName.StartWith(alias);
            });
            if (result.IsNotNull())
            {
                return result;
            }

            return null;
        }
    }
}
