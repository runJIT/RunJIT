using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using Solution.Parser.Project;

namespace RunJit.Cli.RunJit.Generate.Client
{
    internal static class AddClientStructureWriterExtension
    {
        internal static void AddClientStructureWriter(this IServiceCollection services)
        {
            services.AddApiFolderBuilder();
            services.AddFacadeStructureBuilder();

            services.AddSingletonIfNotExists<ClientStructureWriter>();
        }
    }

    internal sealed class ClientStructureWriter(ApiFolderBuilder apiFolderBuilder,
                                         FacadeStructureBuilder facadeStructureBuilder)
    {
        public async Task WriteFileStructureAsync(GeneratedClient client,
                                                  ProjectFile clientProject,
                                                  string projectName,
                                                  string clientName)
        {
            // 0. Create Api folder
            var apiFolder = apiFolderBuilder.Build(clientProject);

            // 1. Domain structure like Users, Resources and more
            await facadeStructureBuilder.CreateAsync(apiFolder, client.Facades, projectName,
                                                     clientName).ConfigureAwait(false);
        }
    }
}
