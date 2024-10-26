using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    public static class AddNameSpaceCollectorExtension
    {
        public static void AddNameSpaceCollector(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<NameSpaceCollector>();
        }
    }

    internal sealed class NameSpaceCollector
    {
        private readonly List<string> _items = new List<string>();

        public void Add(string nameSpace)
        {
            _items.Add(nameSpace);
        }

        public IEnumerable<string> GetAll()
        {
            return _items;
        }
    }
}
