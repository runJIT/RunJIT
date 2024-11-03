using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Generate.DotNetTool.Models;

namespace RunJit.Cli.Generate.DotNetTool
{
    internal static class AddCreateCommandClassesExtension
    {
        internal static void AddCreateCommandClasses(this IServiceCollection services)
        {
            services.AddCommandServiceStructureBuilder();
            services.AddCommandStructureBuilder();
            services.AddCreateArgumentStructure();
            services.AddCreateOptionsStructure();
            services.AddCreateParameterClassStructure();
            services.AddCreateSubCommandStructure();


            services.AddSingletonIfNotExists<CreateCommandClasses>();
        }
    }

    internal sealed class CreateCommandClasses(IEnumerable<IBuildCommandFileStructure> commandFileStructures)
    {
        public void Invoke(string projectName,
                           CommandInfo commandInfo,
                           DirectoryInfo rootDirectory,
                           CommandTypeCollector commandTypeCollector,
                           string currentPath,
                           NameSpaceCollector namespaceCollector,
                           DotNetToolInfos donNetToolName,
                           CommandInfo? parentCommand = null)
        {
            var currentRootPath = $"{currentPath}";

            currentPath = $"{currentRootPath}.{commandInfo.NormalizedName}";

            namespaceCollector.Add(currentPath);
            var subCommnandDirectoryInfo = new DirectoryInfo(Path.Combine(rootDirectory.FullName, commandInfo.NormalizedName));
            subCommnandDirectoryInfo.Create();

            commandFileStructures.ForEach(structure => structure.Create(projectName, parentCommand, commandTypeCollector,
                                                                        currentPath, namespaceCollector, subCommnandDirectoryInfo,
                                                                        commandInfo, donNetToolName));


            foreach (var commandInfoSubCommand in commandInfo.SubCommands)
            {
                Invoke(projectName, commandInfoSubCommand, subCommnandDirectoryInfo, commandTypeCollector, currentPath, namespaceCollector, donNetToolName, commandInfo);
            }
        }
    }
}
