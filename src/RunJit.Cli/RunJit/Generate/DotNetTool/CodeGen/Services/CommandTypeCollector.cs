using Extensions.Pack;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    // Hint, System.CommandLine.Experimental needs one root command with one sub command
    // But we want to use https://docs.microsoft.com/de-de/dotnet/core/tools/extensibility
    // to reuse the 'dotnet' root command. So that it feels like this command comes direct
    // from the dotnet cli.
    // 
    // Default: 'dotnet newtool --use-visualstudio'
    internal sealed class CommandTypeCollector
    {
        private readonly Dictionary<string, IEnumerable<TypeToRegister>> _typesToRegister;

        public CommandTypeCollector()
        {
            _typesToRegister = new Dictionary<string, IEnumerable<TypeToRegister>>();
        }

        public void Add(CommandInfo parameterInfo,
                        TypeToRegister typeToRegister)
        {
            var name = parameterInfo.Name;
            var alreadyExists = _typesToRegister.ContainsKey(name);

            if (alreadyExists)
            {
                _typesToRegister[name] = _typesToRegister[name].Concat(typeToRegister);
            }
            else
            {
                _typesToRegister[name] = typeToRegister.ToIList();
            }
        }

        public Dictionary<string, IEnumerable<TypeToRegister>> GetAll()
        {
            return _typesToRegister;
        }
    }
}
