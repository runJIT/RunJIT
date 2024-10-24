using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;

namespace RunJit.Cli.RunJit.Generate.DotNetTool.CodeGen
{
    public static class AddProgramCodeGenExtension
    {
        public static void AddProgramCodeGen(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<INetToolCodeGen, ProgramCodeGen>();
        }
    }

    internal class ProgramCodeGen(IConsoleService consoleService) : INetToolCodeGen
    {
        private const string template = """
                                        namespace RunJit.Cli
                                        {
                                            public static class Program
                                            {
                                                public static Task<int> Main(string[] args)
                                                {
                                                    return new AppBuilder().Build().RunAsync(args);
                                                }
                                            }
                                        }
                                        
                                        """;

        public async Task GenerateAsync(FileInfo projectFileInfo,
                                        DotNetToolInfos dotNetToolInfos)
        {
            // 1. Add AppBuilder.cs
            var file = Path.Combine(projectFileInfo.Directory!.FullName, "Program.cs");
            await File.WriteAllTextAsync(file, template).ConfigureAwait(false);

            // 2. Print success message
            consoleService.WriteSuccess($"Successfully created {file}");
        }
    }
}
