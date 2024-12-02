using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.Generate.DotNetTool
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

    internal sealed class CommandHandlerBuilder(IEnumerable<ICommandHandlerStringBuilder> commandHandlerStringBuilders)
    {
        public string Build(Models.CommandInfo parameterInfo)
        {
            var builder = commandHandlerStringBuilders.Single(b => b.IsThisBuilderFor(parameterInfo));

            return builder.Build(parameterInfo);
        }
    }
}
