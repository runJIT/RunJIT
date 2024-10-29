using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal static class AddCommandHandlerBuilderExtension
    {
        internal static void AddCommandHandlerBuilder(this IServiceCollection services)
        {
            services.AddCommandHandlerNoArgsAndNoOptionBuilder();
            services.AddCommandHandlerWithArgsOrOptionBuilder();

            services.AddSingletonIfNotExists<CommandHandlerBuilder>();
        }
    }

    internal sealed class CommandHandlerBuilder
    {
        private readonly IEnumerable<ICommandHandlerStringBuilder> _commandHandlerStringBuilders;

        public CommandHandlerBuilder(IEnumerable<ICommandHandlerStringBuilder> commandHandlerStringBuilders)
        {
            _commandHandlerStringBuilders = commandHandlerStringBuilders;
        }

        public string Build(CommandInfo parameterInfo)
        {
            var builder = _commandHandlerStringBuilders.Single(b => b.IsThisBuilderFor(parameterInfo));

            return builder.Build(parameterInfo);
        }
    }
}
