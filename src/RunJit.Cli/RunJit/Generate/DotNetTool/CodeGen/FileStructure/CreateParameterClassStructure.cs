using Extensions.Pack;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal sealed class CreateParameterClassStructure(IParameterClassBuilder parameterClassBuilder) : IBuildCommandFileStructure
    {
        public void Create(string projectName,
                           CommandInfo parameter,
                           CommandTypeCollector commandTypeCollector,
                           string currentPath,
                           NameSpaceCollector namespaceCollector,
                           DirectoryInfo subCommnandDirectoryInfo,
                           CommandInfo subCommand)
        {
            if (!ObjectExtensions.IsNotNull((object?)subCommand.Argument) && !subCommand.Options.Any() && !subCommand.SubCommands.IsNullOrEmpty())
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
