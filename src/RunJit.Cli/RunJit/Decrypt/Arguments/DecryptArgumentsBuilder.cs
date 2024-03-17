using System.CommandLine;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Decrypt
{
    public static class AddDecryptArgumentsBuilderExtension
    {
        public static void AddDecryptArgumentsBuilder(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<IDecryptArgumentsBuilder, DecryptArgumentsBuilder>();
        }
    }

    internal interface IDecryptArgumentsBuilder
    {
        IEnumerable<System.CommandLine.Argument> Build();
    }

    internal class DecryptArgumentsBuilder : IDecryptArgumentsBuilder
    {
        public IEnumerable<System.CommandLine.Argument> Build()
        {
            yield return BuildValueArgument();
        }

        private System.CommandLine.Argument BuildValueArgument()
        {
            return new Argument<string>("value") { Description = "The value which should be decrypted" };
        }
    }
}
