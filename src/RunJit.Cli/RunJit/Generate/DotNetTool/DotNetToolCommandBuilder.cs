using System.CommandLine;
using System.CommandLine.Invocation;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal static class AddDotNetToolCommandBuilderExtension
    {
        internal static void AddDotNetToolCommandBuilder(this IServiceCollection services)
        {
            services.AddDotNetTool();
            services.AddDotNetToolOptionsBuilder();

            services.AddSingletonIfNotExists<IGenerateSubCommandBuilder, DotNetToolCommandBuilder>();
        }
    }

    internal sealed class DotNetToolCommandBuilder(IDotNetToolGen clientGen,
                                            IDotNetToolGenOptionsBuilder optionsBuilder) : IGenerateSubCommandBuilder
    {
        public Command Build()
        {
            var command = new Command(".nettool", "The command to generate a new .net client into a .net web api project");
            optionsBuilder.Build().ToList().ForEach(option => command.AddOption(option));

            command.Handler = CommandHandler.Create<bool, bool, FileInfo, string>((usevisualstudio,
                                                                                   build,
                                                                                   solution,
                                                                                   toolName) => clientGen.HandleAsync(new DotNetToolParameters(usevisualstudio, build, solution,
                                                                                                                                               toolName)));

            return command;
        }
    }
}
