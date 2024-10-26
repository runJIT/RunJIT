using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    public static class AddCommandHandlerBuilderExtension
    {
        public static void AddCommandHandlerBuilder(this IServiceCollection services)
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
