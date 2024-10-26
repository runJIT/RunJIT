using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    public static class AddCommandStructureBuilderExtension
    {
        public static void AddCommandStructureBuilder(this IServiceCollection services)
        {
            services.AddCommandBuilderSimple();
            services.AddCommandBuilderWithOptions();
            services.AddCommandBuilderWithArgument();
            services.AddCommandBuilderWithArgumentAndOption();
            services.AddTypeService();

            services.AddSingletonIfNotExists<IBuildCommandFileStructure, CommandStructureBuilder>();
        }
    }

    internal sealed class CommandStructureBuilder(CommandBuilderSimple commandBuilderSimple,
                                                  CommandBuilderWithOptions commandBuilderWithOptions,
                                                  CommandBuilderWithArgument commandBuilderWithArgument,
                                                  CommandBuilderWithArgumentAndOption commandBuilderWithArgumentAndOption,
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
            if (subCommand.SubCommands.IsNotNull() && subCommand.SubCommands.Any())
            {
                return;
            }

            string? command = null;

            if (ObjectExtensions.IsNull((object?)subCommand.Argument) && subCommand.Options.IsEmpty())
            {
                command = commandBuilderSimple.Build(projectName, subCommand, parameter,
                                                     currentPath);
            }
            else if (ObjectExtensions.IsNull((object?)subCommand.Argument) && subCommand.Options.Any())
            {
                command = commandBuilderWithOptions.Build(projectName, subCommand, parameter,
                                                          currentPath);
            }
            else if (ObjectExtensions.IsNotNull((object?)subCommand.Argument) && subCommand.Options.IsEmpty())
            {
                command = commandBuilderWithArgument.Build(projectName, subCommand, parameter,
                                                           currentPath);
            }
            else if (ObjectExtensions.IsNotNull((object?)subCommand.Argument) && subCommand.Options.Any())
            {
                command = commandBuilderWithArgumentAndOption.Build(projectName, subCommand, parameter,
                                                                    currentPath);
            }

            var fileInfo = new FileInfo(Path.Combine(subCommnandDirectoryInfo.FullName, $"{subCommand.NormalizedName}CommandBuilder.cs"));
            File.WriteAllText(fileInfo.FullName, command);

            var splittedNamespace = currentPath.Split(".").ToList();
            splittedNamespace.Remove(splittedNamespace.Last());
            var newNamespaceForInterface = splittedNamespace.Flatten(".");

            var interfaceName = $"{newNamespaceForInterface}.I{parameter.NormalizedName}SubCommandBuilder";
            var implementationToRegister = typeService.GetFullQualifiedName(projectName, fileInfo);

            commandTypeCollector.Add(subCommand, new TypeToRegister(interfaceName, implementationToRegister));
        }
    }
}
