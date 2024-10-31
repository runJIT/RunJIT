using Argument.Check;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal static class AddParameterClassBuilderExtension
    {
        internal static void AddParameterClassBuilder(this IServiceCollection services)
        {
            services.AddParameterWithArgsOrOptionsClassBuilder();
            services.AddParameterWithoutArgsOrOptionsClassBuilder();

            services.AddSingletonIfNotExists<ParameterClassBuilder>();
        }
    }

    internal sealed class ParameterClassBuilder 
    {
        private readonly IEnumerable<IParameterSpecificClassBuilder> _parameterSpecificClassBuilders;

        public ParameterClassBuilder(IEnumerable<IParameterSpecificClassBuilder> parameterSpecificClassBuilders)
        {
            Throw.IfNull(() => parameterSpecificClassBuilders);

            _parameterSpecificClassBuilders = parameterSpecificClassBuilders;
        }

        public string Build(string projectName,
                            Models.CommandInfo parameterInfo,
                            string nameSpace)
        {
            Throw.IfNullOrWhiteSpace(projectName);
            Throw.IfNull(() => parameterInfo);
            Throw.IfNullOrWhiteSpace(nameSpace);

            var builder = _parameterSpecificClassBuilders.Single(b => b.IsThisBuilderFor(parameterInfo));

            return builder.Build(projectName, parameterInfo, nameSpace);
        }
    }
}
