using Argument.Check;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal sealed class ParameterClassBuilder : IParameterClassBuilder
    {
        private readonly IEnumerable<IParameterSpecificClassBuilder> _parameterSpecificClassBuilders;

        public ParameterClassBuilder(IEnumerable<IParameterSpecificClassBuilder> parameterSpecificClassBuilders)
        {
            Throw.IfNull(() => parameterSpecificClassBuilders);

            _parameterSpecificClassBuilders = parameterSpecificClassBuilders;
        }

        public string Build(string projectName, CommandInfo parameterInfo, string nameSpace)
        {
            Throw.IfNullOrWhiteSpace(projectName);
            Throw.IfNull(() => parameterInfo);
            Throw.IfNullOrWhiteSpace(nameSpace);

            var builder = _parameterSpecificClassBuilders.Single(b => b.IsThisBuilderFor(parameterInfo));
            return builder.Build(projectName, parameterInfo, nameSpace);
        }
    }
}
