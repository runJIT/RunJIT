using System.CommandLine;
using System.CommandLine.Invocation;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Encrypt
{
    public static class AddEncryptCommandBuilderExtension
    {
        public static void AddEncryptCommandBuilder(this IServiceCollection services)
        {
            services.AddEncryptArgumentsBuilder();
            services.AddEncryptService();

            services.AddSingletonIfNotExists<IRunJitSubCommandBuilder, EncryptCommandBuilder>();
        }
    }

    internal sealed class EncryptCommandBuilder(
        IEncryptArgumentsBuilder encryptArgumentsBuilder,
        IEncryptService encryptService)
        : IRunJitSubCommandBuilder
    {
        public Command Build()
        {
            var newCommand = new Command("encrypt", "The command encrypt some sepcial strings :)");
            var arguments = encryptArgumentsBuilder.Build();
            arguments.ToList().ForEach(argument => newCommand.AddArgument(argument));
            newCommand.Handler = CommandHandler.Create<string>(value => encryptService.HandleAsync(new EncryptParameters(value)));
            return newCommand;
        }
    }
}
