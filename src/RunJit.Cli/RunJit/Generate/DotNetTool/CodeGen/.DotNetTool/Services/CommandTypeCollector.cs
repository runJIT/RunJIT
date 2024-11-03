using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.Generate.DotNetTool
{
    internal static class AddCommandTypeCollectorExtension
    {
        internal static void AddCommandTypeCollector(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<CommandTypeCollector>();
        }
    }

    // Hint, System.CommandLine.Experimental needs one root command with one sub command
    // But we want to use https://docs.microsoft.com/de-de/dotnet/core/tools/extensibility
    // to reuse the 'dotnet' root command. So that it feels like this command comes direct
    // from the dotnet cli.
    // 
    // Default: 'dotnet newtool --use-visualstudio'
    internal sealed class CommandTypeCollector
    {
        private readonly Dictionary<string, IEnumerable<Models.TypeToRegister>> _typesToRegister;

        public CommandTypeCollector()
        {
            _typesToRegister = new Dictionary<string, IEnumerable<Models.TypeToRegister>>();
        }

        public void Add(Models.CommandInfo parameterInfo,
                        Models.TypeToRegister typeToRegister)
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

        public Dictionary<string, IEnumerable<Models.TypeToRegister>> GetAll()
        {
            return _typesToRegister;
        }
    }
}
