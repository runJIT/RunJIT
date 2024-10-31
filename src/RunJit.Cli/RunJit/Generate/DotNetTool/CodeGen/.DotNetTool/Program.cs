using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;
using Solution.Parser.CSharp;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal static class AddProgramCodeGenExtension
    {
        internal static void AddProgramCodeGen(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<IDotNetToolSpecificCodeGen, ProgramCodeGen>();
        }
    }

    internal sealed class ProgramCodeGen(ConsoleService consoleService) : IDotNetToolSpecificCodeGen
    {
        private const string Template = """
                                        namespace $namespace$
                                        {
                                            // Needed public to run automated tests which are debugable
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
                                        Models.DotNetToolInfos dotNetToolInfos)
        {
            // 1. Add AppBuilder.cs
            var file = Path.Combine(projectFileInfo.Directory!.FullName, "Program.cs");
            

            var newTemplate = Template.Replace("$namespace$", dotNetToolInfos.ProjectName)
                                      .Replace("$dotNetToolName$", dotNetToolInfos.NormalizedName);

            var formattedTemplate = newTemplate.FormatSyntaxTree();

            await File.WriteAllTextAsync(file, formattedTemplate).ConfigureAwait(false);

            // 2. Print success message
            consoleService.WriteSuccess($"Successfully created {file}");
        }
    }
}
