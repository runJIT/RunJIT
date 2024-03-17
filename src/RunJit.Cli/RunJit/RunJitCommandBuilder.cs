using System.CommandLine;
using Extensions.Pack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.RunJit.Check;
using RunJit.Cli.RunJit.Decrypt;
using RunJit.Cli.RunJit.Encrypt;
using RunJit.Cli.RunJit.Fix;
using RunJit.Cli.RunJit.Rename;
using RunJit.Cli.RunJit.Update;
using RunJit.Cli.RunJit.Zip;

namespace RunJit.Cli.RunJit
{
    internal static class AddRunJitCommandBuilderExtension
    {
        internal static void AddRunJitCommandBuilder(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEncryptCommandBuilder();
            services.AddDecryptCommandBuilder();
            services.AddZipCommandBuilder();
            services.AddUpdateCommandBuilder();
            services.AddRenameCommandBuilder();
            services.AddFixCommandBuilder();
            services.AddCheckCommandBuilder();

            services.AddSingletonIfNotExists<IRunJitCommandBuilder, RunJitCommandBuilder>();
        }
    }

    internal interface IRunJitCommandBuilder
    {
        RootCommand Build();
    }

    internal class RunJitCommandBuilder(IEnumerable<IRunJitSubCommandBuilder> dotnetSubCommandBuilders) : IRunJitCommandBuilder
    {
        public RootCommand Build()
        {
            var rootCommand = new RootCommand { Name = "runjit", Description = @"Run 'dotnet [command] --help' in order to get specific information." };
            dotnetSubCommandBuilders.ToList().ForEach(builder => rootCommand.AddCommand(builder.Build()));
            return rootCommand;
        }
    }
}
