using Extensions.Pack;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal sealed class CommandServiceStructureBuilder(ICommandServiceBuilder commandServiceBuilder,
                                                         ICommandServiceInterfaceBuilder commandServiceInterfaceBuilder,
                                                         ITypeService typeService)
        : IBuildCommandFileStructure
    {
        public void Create(string projectName, CommandInfo parameter, CommandTypeCollector commandTypeCollector, string currentPath, NameSpaceCollector namespaceCollector, DirectoryInfo subCommnandDirectoryInfo, CommandInfo subCommand)
        {
            if (subCommand.SubCommands.IsNotNull() && subCommand.SubCommands.Any())
            {
                return;
            }

            var commandServiceResult = commandServiceBuilder.Build(projectName, subCommand, currentPath);
            var serviceFolder = new DirectoryInfo(Path.Combine(subCommnandDirectoryInfo.FullName, "Service"));
            serviceFolder.Exists.IfFalseThen(() => serviceFolder.Create());
            var commandService = new FileInfo(Path.Combine(serviceFolder.FullName, $"{subCommand.NormalizedName}Service.cs"));

            File.WriteAllText(commandService.FullName, commandServiceResult);

            var serviceInterface = commandServiceInterfaceBuilder.Build(projectName, subCommand, currentPath);
            var commandServiceInterface = new FileInfo(Path.Combine(serviceFolder.FullName, $"I{subCommand.NormalizedName}Service.cs"));

            File.WriteAllText(commandServiceInterface.FullName, serviceInterface);

            var interfaceToRegister = typeService.GetFullQualifiedName(projectName, commandServiceInterface);
            var implementationToRegister = typeService.GetFullQualifiedName(projectName, commandService);

            commandTypeCollector.Add(subCommand, new TypeToRegister(interfaceToRegister, implementationToRegister));
        }
    }
}
