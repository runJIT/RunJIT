using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    public static class AddCommandServiceStructureBuilderExtension
    {
        public static void AddCommandServiceStructureBuilder(this IServiceCollection services)
        {
            services.AddCommandServiceBuilder();
            services.AddCommandServiceInterfaceBuilder();
            services.AddTypeService();

            services.AddSingletonIfNotExists<IBuildCommandFileStructure, CommandServiceStructureBuilder>();
        }
    }

    internal sealed class CommandServiceStructureBuilder(CommandServiceBuilder commandServiceBuilder,
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
            if (commandInfo.SubCommands.IsNotNull() && commandInfo.SubCommands.Any())
            {
                return;
            }

            var commandServiceResult = commandServiceBuilder.Build(projectName, commandInfo, currentPath);
            var serviceFolder = new DirectoryInfo(Path.Combine(subCommnandDirectoryInfo.FullName, "Service"));
            serviceFolder.Exists.IfFalseThen(() => serviceFolder.Create());
            var commandService = new FileInfo(Path.Combine(serviceFolder.FullName, $"{commandInfo.NormalizedName}Service.cs"));

            File.WriteAllText(commandService.FullName, commandServiceResult);
            
            var implementationToRegister = typeService.GetFullQualifiedName(projectName, commandService);

            commandTypeCollector.Add(commandInfo, new TypeToRegister(implementationToRegister, implementationToRegister));
        }
    }
}
