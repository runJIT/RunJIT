using System.CommandLine;
using System.CommandLine.Invocation;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Zip
{
    public static class AddZipCommandBuilderExtension
    {
        public static void AddZipCommandBuilder(this IServiceCollection services)
        {
            services.AddZipOptionsBuilder();
            services.AddZipArgumentsBuilder();
            services.AddZipService();

            services.AddSingletonIfNotExists<IRunJitSubCommandBuilder, ZipCommandBuilder>();
        }
    }

    internal sealed class ZipCommandBuilder(IZipArgumentsBuilder zipArgumentsBuilder,
                                            IZipOptionsBuilder zipOptionsBuilder,
                                            IZipService zipService) : IRunJitSubCommandBuilder
    {
        public Command Build()
        {
            var newCommand = new Command("zip", "The command to zip a folder into a zip file");
            var arguments = zipArgumentsBuilder.Build();
            var options = zipOptionsBuilder.Build();

            arguments.ForEach(argument => newCommand.AddArgument(argument));
            options.ForEach(option => newCommand.AddOption(option));

            newCommand.Handler = CommandHandler.Create<DirectoryInfo, FileInfo>((directory,
                                                                                 zipFile) => zipService.ZipAsync(new ZipParameters(directory, zipFile)));

            return newCommand;
        }
    }
}
