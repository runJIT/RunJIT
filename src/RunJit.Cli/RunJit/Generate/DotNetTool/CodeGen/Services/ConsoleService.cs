using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;

namespace RunJit.Cli.RunJit.Generate.DotNetTool.CodeGen
{
    public static class AddConsoleServiceCodeGenExtension
    {
        public static void AddConsoleServiceCodeGen(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<INetToolCodeGen, ConsoleServiceCodeGen>();
        }
    }

    internal class ConsoleServiceCodeGen(IConsoleService consoleService) : INetToolCodeGen
    {
        private const string template = """
                                        using Extensions.Pack;
                                        using Microsoft.Extensions.DependencyInjection;
                                        
                                        namespace RunJit.Cli.Services
                                        {
                                            internal static class AddConsoleServiceExtension
                                            {
                                                internal static void AddConsoleService(this IServiceCollection services)
                                                {
                                                    services.AddSingletonIfNotExists<ConsoleService>();
                                                }
                                            }
                                        
                                            internal class ConsoleService
                                            {
                                                public void WriteLine()
                                                {
                                                    Console.WriteLine();
                                                }
                                        
                                                public void WriteInfo(string value)
                                                {
                                                    WriteLine(value);
                                                }
                                        
                                                public void WriteInput(string value)
                                                {
                                                    Console.ForegroundColor = ConsoleColor.Green;
                                                    WriteLine(value);
                                                    Console.ForegroundColor = ConsoleColor.White;
                                                }
                                        
                                                public void WriteSuccess(string value)
                                                {
                                                    Console.ForegroundColor = ConsoleColor.Green;
                                                    WriteLine(value);
                                                    Console.ForegroundColor = ConsoleColor.White;
                                                }
                                        
                                                public string ReadLine()
                                                {
                                                    var result = Console.ReadLine();
                                                    Console.WriteLine();
                                        
                                                    return result ?? string.Empty;
                                                }
                                        
                                                public void WriteSample(string value)
                                                {
                                                    Console.ForegroundColor = ConsoleColor.Gray;
                                                    Console.WriteLine(value);
                                                    Console.WriteLine();
                                                    Console.ForegroundColor = ConsoleColor.White;
                                                }
                                        
                                                public void WriteError(string value)
                                                {
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    WriteLine(value);
                                                    Console.ForegroundColor = ConsoleColor.White;
                                                }
                                        
                                                private void WriteLine(string value)
                                                {
                                                    Console.WriteLine(value);
                                                }
                                            }
                                        }
                                        
                                        """;

        public async Task GenerateAsync(FileInfo projectFileInfo,
                                        DotNetToolInfos dotNetToolInfos)
        {
            // 1. Add Services Folder
            var appFolder = new DirectoryInfo(Path.Combine(projectFileInfo.Directory!.FullName, "Services"));
            if (appFolder.NotExists())
            {
                appFolder.Create();
            }

            // 2. Add ConsoleService.cs
            var file = Path.Combine(appFolder.FullName, "ConsoleService.cs");
            await File.WriteAllTextAsync(file, template).ConfigureAwait(false);

            // 3. Print success message
            consoleService.WriteSuccess($"Successfully created {file}");
        }
    }
}
