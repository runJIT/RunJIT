using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    public static class AddCreateArgumentStructureExtension
    {
        public static void AddCreateArgumentStructure(this IServiceCollection services)
        {
            services.AddArgumentInterfaceBuilder();
            services.AddArgumentBuilder();
            services.AddTypeService();

            services.AddSingletonIfNotExists<IBuildCommandFileStructure, CreateArgumentStructure>();
        }
    }

    internal sealed class CreateArgumentStructure(ArgumentInterfaceBuilder argumentInterfaceBuilder,
                                                  ArgumentBuilder argumentBuilder,
                                                  TypeService typeService)
        : IBuildCommandFileStructure
    {
        public void Create(string projectName,
                           CommandInfo parameter,
                           CommandTypeCollector commandTypeCollector,
                           string currentPath,
                           NameSpaceCollector namespaceCollector,
                           DirectoryInfo subCommnandDirectoryInfo,
                           CommandInfo subCommand)
        {
            if (subCommand.Argument.IsNull())
            {
                return;
            }

            var argumentFolder = new DirectoryInfo(Path.Combine(subCommnandDirectoryInfo.FullName, "Arguments"));
            argumentFolder.Create();

            var argumentInterfaceSyntaxTree = argumentInterfaceBuilder.Build(projectName, subCommand, currentPath);
            var argumentInterfaceFilePath = new FileInfo(Path.Combine(argumentFolder.FullName, $"I{subCommand.NormalizedName}ArgumentBuilder.cs"));
            File.WriteAllText(argumentInterfaceFilePath.FullName, argumentInterfaceSyntaxTree);

            var argumentImplementationSyntaxTree = argumentBuilder.Build(projectName, subCommand, currentPath);
            var argumentImplementationFilePath = new FileInfo(Path.Combine(argumentFolder.FullName, $"{subCommand.NormalizedName}ArgumentBuilder.cs"));
            File.WriteAllText(argumentImplementationFilePath.FullName, argumentImplementationSyntaxTree);

            var interfaceToRegister = typeService.GetFullQualifiedName(projectName, argumentInterfaceFilePath);
            var implementationToRegister = typeService.GetFullQualifiedName(projectName, argumentImplementationFilePath);

            commandTypeCollector.Add(subCommand, new TypeToRegister(interfaceToRegister, implementationToRegister));

            namespaceCollector.Add($"{currentPath}.Arguments");
        }
    }
}
