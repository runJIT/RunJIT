using Microsoft.Extensions.DependencyInjection;
using Extensions.Pack;

namespace RunJit.Cli.Database
{
    public static class AddTableNameFinderExtension
    {
        public static void AddTableNameFinder(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<TableNameFinder>();
        }
    }

    // KISS
    internal class TableNameFinder
    {
        public IEnumerable<string> FindAllTables(string sql)
        {
            var tableNamePreOps = new string[] { "FROM", "JOIN" };

            var sqlParts = sql.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < sqlParts.Length; i++)
            {
                var sqlPart = sqlParts[i].ToUpperInvariant();
                if (tableNamePreOps.Contains(sqlPart))
                {
                    yield return sqlParts[i + 1].Trim(';');
                }
            }
        }
    }
}
