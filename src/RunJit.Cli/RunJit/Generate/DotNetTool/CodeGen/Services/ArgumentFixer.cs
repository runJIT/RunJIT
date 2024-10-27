using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;
using Solution.Parser.CSharp;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    public static class AddArgumentFixerCodeGenExtension
    {
        public static void AddArgumentFixerCodeGen(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<INetToolCodeGen, ArgumentFixerCodeGen>();
        }
    }

    internal class ArgumentFixerCodeGen(ConsoleService consoleService, 
                                        NamespaceProvider namespaceProvider) : INetToolCodeGen
    {
        private const string Template = """
                                        using Extensions.Pack;
                                        using Microsoft.Extensions.DependencyInjection;

                                        namespace $namespace$
                                        {
                                            internal static class Add$dotNetToolName$ArgumentFixerExtension
                                            {
                                                internal static void Add$dotNetToolName$ArgumentFixer(this IServiceCollection services)
                                                {
                                                    services.AddSingletonIfNotExists<$dotNetToolName$ArgumentFixer>();
                                                }
                                            }
                                        
                                            internal sealed class $dotNetToolName$ArgumentFixer
                                            {
                                                public string[] Fix(string[] args)
                                                {
                                                    var defaultArgs = new[] { "runjit" };
                                                    var newArgs = defaultArgs.Concat(args).Distinct().ToList();
                                        
                                                    return newArgs.ToArray();
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

            // 2. Add ArgumentFixer.cs
            var file = Path.Combine(appFolder.FullName, "ArgumentFixer.cs");

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
