using System.CommandLine.Invocation;
using Extensions.Pack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;

namespace RunJit.Cli.RunJit.Generate.Client
{
    internal static class AddClientExtension
    {
        internal static void AddClient(this IServiceCollection services,
                                       IConfiguration configuration)
        {
            services.AddConsoleService();
            services.AddDotNetToolToolBuildFromStrategy();
            services.AddClientCreator();
            services.AddProcessService();
            services.AddTargetFolderService();
            services.AddTemplateExtractor(configuration);
            services.AddTemplateService();
            services.AddBuildClientFromConsole();

            services.AddSingletonIfNotExists<IClientGen, ClientGen>();
        }
    }

    internal interface IClientGen
    {
        Task<int> HandleAsync(ClientParameters parameters);
    }

    internal class ClientGen(ITargetFolderService targetFolderService,
                             ITemplateExtractor templateExtractor,
                             ITemplateService templateService,
                             IProcessService processService,
                             ConsoleService consoleService,
                             ClientGeneratorBuilder clientGeneratorBuilder,
                             ClientCreator clientCreator)
        : IClientGen
    {
        public async Task<int> HandleAsync(ClientParameters parameters)
        {
            // ToDo: Idea a new parameter to control with or without build :)
            // 0. Build the target solution first
            if (parameters.Build)
            {
                var dotnetBuildResult = await processService.RunAsync("dotnet", $"build {parameters.SolutionFile.FullName}").ConfigureAwait(false);

                if (dotnetBuildResult.ExitCode != 0)
                {
                    return dotnetBuildResult.ExitCode;
                }
            }

            // 1. Build client generator from parameters
            var client = clientGeneratorBuilder.BuildFrom(parameters);

            // 2. Create target structure if needed / cleanup
            var targetDirectory = targetFolderService.CreateTargetDirectory(client);

            // 3. Extract templates solution or project templates depends on parameters
            await templateExtractor.ExtractToAsync(targetDirectory, parameters).ConfigureAwait(false);

            // 4. Renaming all stuff
            templateService.RenameAllIn(targetDirectory, client);

            // Remove templates if someone is orphan
            targetDirectory.EnumerateDirectories("rps.template*").ForEach(directory => directory.Delete(true));

            // 5. Find the created solution or use source one, dependent on client generation
            var solutionFile = targetFolderService.GetSolutionFile(client, targetDirectory);

            // 7. Generate client code
            await clientCreator.GenerateClientAsync(client, solutionFile).ConfigureAwait(false);

            // important if solution running and opened
            await Process.ExecuteAsync("dotnet", $"restore {solutionFile.FullName}").ConfigureAwait(false);

            consoleService.WriteSuccess($"Enjoy your new generated: '{client.ProjectName}' client");

            return 0;
        }
    }
}
