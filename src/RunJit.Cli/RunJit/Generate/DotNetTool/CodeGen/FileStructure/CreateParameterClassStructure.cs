using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    public static class AddCreateParameterClassStructureExtension
    {
        public static void AddCreateParameterClassStructure(this IServiceCollection services)
        {
            services.AddParameterClassBuilder();

            services.AddSingletonIfNotExists<IBuildCommandFileStructure, CreateParameterClassStructure>();
        }
    }

    internal sealed class CreateParameterClassStructure(ParameterClassBuilder parameterClassBuilder) : IBuildCommandFileStructure
    {
        public void Create(string projectName,
                           CommandInfo parameter,
                           CommandTypeCollector commandTypeCollector,
                           string currentPath,
                           NameSpaceCollector namespaceCollector,
                           DirectoryInfo subCommnandDirectoryInfo,
                           CommandInfo subCommand)
        {
            if (!subCommand.Argument.IsNotNull() && !subCommand.Options.Any() && !subCommand.SubCommands.IsNullOrEmpty())
            {
                return;
            }

            var parameterModelClass = parameterClassBuilder.Build(projectName, subCommand, currentPath);
            var parameterClassFileInfo = new FileInfo(Path.Combine(subCommnandDirectoryInfo.FullName, $"{subCommand.NormalizedName}Parameters.cs"));
            File.WriteAllText(parameterClassFileInfo.FullName, parameterModelClass);
            namespaceCollector.Add($"{currentPath}.Service");
        }
    }
}
