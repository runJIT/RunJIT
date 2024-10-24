namespace RunJit.Cli.RunJit.Generate.DotNetTool.CodeGen
{
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
