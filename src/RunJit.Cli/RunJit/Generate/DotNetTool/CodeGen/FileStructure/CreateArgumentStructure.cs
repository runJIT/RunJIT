using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    public static class AddCreateArgumentStructureExtension
    {
        public static void AddCreateArgumentStructure(this IServiceCollection services)
        {
            services.AddArgumentBuilder();
            services.AddTypeService();

            services.AddSingletonIfNotExists<IBuildCommandFileStructure, CreateArgumentStructure>();
        }
    }

    internal sealed class CreateArgumentStructure(ArgumentBuilder argumentBuilder,
                                                  TypeService typeService)
        : IBuildCommandFileStructure
    {
        public void Create(string projectName,
                           CommandInfo? parentCommandInfo,
                           CommandTypeCollector commandTypeCollector,
                           string currentPath,
                           NameSpaceCollector namespaceCollector,
                           DirectoryInfo subCommnandDirectoryInfo,
                           CommandInfo commandInfo)
        {
            if (commandInfo.Argument.IsNull())
            {
                return;
            }

            var argumentFolder = new DirectoryInfo(Path.Combine(subCommnandDirectoryInfo.FullName, "Arguments"));
            argumentFolder.Create();

            var argumentImplementationSyntaxTree = argumentBuilder.Build(projectName, commandInfo, currentPath);
            var argumentImplementationFilePath = new FileInfo(Path.Combine(argumentFolder.FullName, $"{commandInfo.NormalizedName}ArgumentBuilder.cs"));
            File.WriteAllText(argumentImplementationFilePath.FullName, argumentImplementationSyntaxTree);

            var implementationToRegister = typeService.GetFullQualifiedName(projectName, argumentImplementationFilePath);

            commandTypeCollector.Add(commandInfo, new TypeToRegister(implementationToRegister, implementationToRegister));

            namespaceCollector.Add($"{currentPath}.Arguments");
        }
    }
}
