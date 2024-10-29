using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;
using Solution.Parser.CSharp;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal static class AddOutputServiceCodeGenExtension
    {
        internal static void AddOutputServiceCodeGen(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<INetToolCodeGen, OutputServiceCodeGen>();
        }
    }

    internal sealed class OutputServiceCodeGen(ConsoleService consoleService, 
                                        NamespaceProvider namespaceProvider) : INetToolCodeGen
    {
        private const string Template = """
                                        using Extensions.Pack;
                                        
                                        namespace $namespace$
                                        {
                                            internal static class AddOutputServiceExtension
                                            {
                                                internal static void AddOutputService(this IServiceCollection services)
                                                {
                                                    services.AddOutputFormatter();
                                                    services.AddOutputWriter();
                                        
                                                    services.AddSingletonIfNotExists<OutputService>();
                                                }
                                            }
                                        
                                            internal sealed class OutputService(OutputFormatter outputFormatter,
                                                                                OutputWriter outputWriter)
                                            {
                                                internal async Task WriteAsync(string value,
                                                                               FileInfo? fileInfo,
                                                                               FormatType formatType)
                                                {
                                                    // 1. Format the string into expected format
                                                    var formattedString = outputFormatter.Format(value, formatType);
                                        
                                                    // 3. Write the formatted string to the output
                                                    await outputWriter.WriteAsync(formattedString, fileInfo).ConfigureAwait(false);
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

            // 2. Add OutputService.cs
            var file = Path.Combine(appFolder.FullName, "OutputService.cs");

            var newTemplate = Template.Replace("$namespace$", dotNetToolInfos.ProjectName)
                                      .Replace("$dotNetToolName$", dotNetToolInfos.DotNetToolName.NormalizedName)
                                      .Replace("$dotnettoolnamelower$", dotNetToolInfos.DotNetToolName.NormalizedName.ToLower());

            var formattedTemplate = newTemplate;

            await File.WriteAllTextAsync(file, formattedTemplate).ConfigureAwait(false);

            // 3. Adjust namespace provider
            namespaceProvider.SetNamespaceProviderAsync(projectFileInfo, $"{dotNetToolInfos.ProjectName}.Services", true);

            // 4. Print success message
            consoleService.WriteSuccess($"Successfully created {file}");
        }
    }
}
