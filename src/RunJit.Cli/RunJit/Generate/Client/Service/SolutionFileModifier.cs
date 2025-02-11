using Argument.Check;
using DotNetTool.Service;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services.Net;

namespace RunJit.Cli.RunJit.Generate.Client
{
    internal static class AddAddSolutionFileModifierExtension
    {
        internal static void AddSolutionFileModifier(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<SolutionFileModifier>();
        }
    }

    internal class SolutionFileModifier(IDotNet dotNet)
    {
        public async Task AddProjectsAsync(FileInfo solutionFile)
        {
            Throw.IfNull(solutionFile.Directory);

            var clientProject = solutionFile.Directory.EnumerateFiles($"{solutionFile.NameWithoutExtension()}.Client.csproj", SearchOption.AllDirectories).FirstOrDefault();
            var clientTestProject = solutionFile.Directory.EnumerateFiles($"{solutionFile.NameWithoutExtension()}.Client.Test.csproj", SearchOption.AllDirectories).FirstOrDefault();
            Throw.IfNull(clientProject);
            Throw.IfNull(clientTestProject);

            await dotNet.AddProjectToSolutionAsync(solutionFile, clientProject, "Client").ConfigureAwait(false);
            await dotNet.AddProjectToSolutionAsync(solutionFile, clientTestProject, "Client").ConfigureAwait(false);
        }
    }
}
