using System.CommandLine;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Encrypt
{
    public static class AddEncryptArgumentsBuilderExtension
    {
        public static void AddEncryptArgumentsBuilder(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<IEncryptArgumentsBuilder, EncryptArgumentsBuilder>();
        }
    }

    internal interface IEncryptArgumentsBuilder
    {
        IEnumerable<System.CommandLine.Argument> Build();
    }

    internal sealed class EncryptArgumentsBuilder : IEncryptArgumentsBuilder
    {
        public IEnumerable<System.CommandLine.Argument> Build()
        {
            yield return BuildValueArgument();
        }

        private System.CommandLine.Argument BuildValueArgument()
        {
            return new Argument<string>("value") { Description = "The value which should be encrypted" };
        }
    }
}
