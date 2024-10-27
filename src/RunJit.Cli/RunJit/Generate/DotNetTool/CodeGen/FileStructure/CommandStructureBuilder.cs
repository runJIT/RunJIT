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
                                                  TypeService typeService) : IBuildCommandFileStructure
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

            string? command = null;
            // ToDo: Better refactor take over code from .net tool builder > 7 years old
            if (commandInfo.Argument.IsNull() && commandInfo.Options.IsEmpty())
            {
                command = commandBuilderSimple.Build(projectName, commandInfo, parentCommandInfo, currentPath);
            }
            else if (commandInfo.Argument.IsNull() && commandInfo.Options.Any())
            {
                command = commandBuilderWithOptions.Build(projectName, commandInfo, parentCommandInfo,
                                                          currentPath);
            }
            else if (commandInfo.Argument.IsNotNull() && commandInfo.Options.IsEmpty())
            {
                command = commandBuilderWithArgument.Build(projectName, commandInfo, parentCommandInfo,
                                                           currentPath);
            }
            else if (commandInfo.Argument.IsNotNull() && commandInfo.Options.Any())
            {
                command = commandBuilderWithArgumentAndOption.Build(projectName, commandInfo, parentCommandInfo, currentPath);
            }

            var fileInfo = new FileInfo(Path.Combine(subCommnandDirectoryInfo.FullName, $"{commandInfo.NormalizedName}CommandBuilder.cs"));
            File.WriteAllText(fileInfo.FullName, command);

            var splittedNamespace = currentPath.Split(".").ToList();
            splittedNamespace.Remove(splittedNamespace.Last());
            var newNamespaceForInterface = splittedNamespace.Flatten(".");

            var interfaceName = $"{newNamespaceForInterface}.I{parentCommandInfo?.NormalizedName}SubCommandBuilder";
            var implementationToRegister = typeService.GetFullQualifiedName(projectName, fileInfo);

            commandTypeCollector.Add(commandInfo, new TypeToRegister(interfaceName, implementationToRegister));
        }
    }
}
