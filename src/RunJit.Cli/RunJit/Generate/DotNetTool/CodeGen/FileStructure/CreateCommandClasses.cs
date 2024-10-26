using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    public static class AddCreateCommandClassesExtension
    {
        public static void AddCreateCommandClasses(this IServiceCollection services)
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
                           CommandInfo parameter,
                           DirectoryInfo rootDirectory,
                           CommandTypeCollector commandTypeCollector,
                           string currentPath,
                           NameSpaceCollector namespaceCollector)
        {
            var subCommands = parameter.SubCommands;

            if (subCommands.IsNull())
            {
                return;
            }

            var currentRootPath = $"{currentPath}.{parameter.NormalizedName}";

            foreach (var subCommand in subCommands)
            {
                currentPath = $"{currentRootPath}.{subCommand.NormalizedName}";

                namespaceCollector.Add(currentPath);
                var subCommnandDirectoryInfo = new DirectoryInfo(Path.Combine(rootDirectory.FullName, subCommand.NormalizedName));
                subCommnandDirectoryInfo.Create();

                var closure = currentPath;

                commandFileStructures.ForEach(structure => structure.Create(projectName, parameter, commandTypeCollector,
                                                                            closure, namespaceCollector, subCommnandDirectoryInfo,
                                                                            subCommand));

                Invoke(projectName, subCommand, subCommnandDirectoryInfo,
                       commandTypeCollector, currentRootPath, namespaceCollector);
            }
        }
    }
}
