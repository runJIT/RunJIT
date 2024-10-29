using System.CommandLine;
using System.CommandLine.Invocation;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Decrypt
{
    internal static class AddDecryptCommandBuilderExtension
    {
        internal static void AddDecryptCommandBuilder(this IServiceCollection services)
        {
            services.AddDecryptArgumentsBuilder();
            services.AddDecryptService();

            services.AddSingletonIfNotExists<IRunJitSubCommandBuilder, PackCommandBuilder>();
        }
    }

    internal sealed class PackCommandBuilder(IDecryptArgumentsBuilder encryptArgumentsBuilder,
                                             IDecryptService decryptService)
        : IRunJitSubCommandBuilder
    {
        public Command Build()
        {
            var newCommand = new Command("decrypt", "The command to decrypt some sepcial strings :)");
            var arguments = encryptArgumentsBuilder.Build();
            arguments.ToList().ForEach(argument => newCommand.AddArgument(argument));
            newCommand.Handler = CommandHandler.Create<string>(value => decryptService.HandleAsync(new DecryptParameters(value)));

            return newCommand;
        }
    }
}
