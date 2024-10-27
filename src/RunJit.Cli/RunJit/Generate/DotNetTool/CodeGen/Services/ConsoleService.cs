using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;
using Solution.Parser.CSharp;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    public static class AddConsoleServiceCodeGenExtension
    {
        public static void AddConsoleServiceCodeGen(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<INetToolCodeGen, ConsoleServiceCodeGen>();
        }
    }

    internal sealed class ConsoleServiceCodeGen(ConsoleService consoleService,
                                         NamespaceProvider namespaceProvider) : INetToolCodeGen
    {
        private const string Template = """
                                        using Extensions.Pack;
                                        using Microsoft.Extensions.DependencyInjection;

                                        namespace $namespace$
                                        {
                                            internal static class AddConsoleServiceExtension
                                            {
                                                internal static void AddConsoleService(this IServiceCollection services)
                                                {
                                                    services.AddSingletonIfNotExists<ConsoleService>();
                                                }
                                            }
                                        
                                            internal sealed class ConsoleService
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

            var newTemplate = Template.Replace("$namespace$", dotNetToolInfos.ProjectName)
                                      .Replace("$dotNetToolName$", dotNetToolInfos.DotNetToolName.NormalizedName);

            var formattedTemplate = newTemplate.FormatSyntaxTree();

            await File.WriteAllTextAsync(file, formattedTemplate).ConfigureAwait(false);

            // 3. Adjust namespace provider
            namespaceProvider.SetNamespaceProviderAsync(projectFileInfo, $"{dotNetToolInfos.ProjectName}.Services", true);

            // 4. Print success message
            consoleService.WriteSuccess($"Successfully created {file}");
        }
    }
}
