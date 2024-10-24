using Extensions.Pack;
using RunJit.Cli.RunJit.Generate.DotNetTool.CodeGen.Models;
using RunJit.Cli.RunJit.Generate.DotNetTool.CodeGen.Parameter;

namespace RunJit.Cli.RunJit.Generate.DotNetTool.CodeGen.FileStructure
{
    internal sealed class CreateParameterClassStructure(IParameterClassBuilder parameterClassBuilder) : IBuildCommandFileStructure
    {
        public void Create(string projectName, CommandInfo parameter, CommandTypeCollector commandTypeCollector, string currentPath, NameSpaceCollector namespaceCollector, DirectoryInfo subCommnandDirectoryInfo, CommandInfo subCommand)
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
