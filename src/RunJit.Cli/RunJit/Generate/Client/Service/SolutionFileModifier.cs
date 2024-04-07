using Argument.Check;
using DotNetTool.Service;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Generate.Client
{
    internal static class AddAddSolutionFileModifierExtension
    {
        internal static void AddSolutionFileModifier(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<SolutionFileModifier>();
        }
    }

    public class SolutionFileModifier
    {
        public async Task AddProjectsAsync(FileInfo solutionFile)
        {
            Throw.IfNull(solutionFile.Directory);

            var clientProject = solutionFile.Directory.EnumerateFiles($"{solutionFile.NameWithoutExtension()}.Client.csproj", SearchOption.AllDirectories).FirstOrDefault();
            var clientTestProject = solutionFile.Directory.EnumerateFiles($"{solutionFile.NameWithoutExtension()}.Client.Test.csproj", SearchOption.AllDirectories).FirstOrDefault();
            Throw.IfNull(clientProject);
            Throw.IfNull(clientTestProject);

            var dotnetTool = DotNetToolFactory.Create();
            var addClientProjectResult = await dotnetTool.RunAsync("dotnet", $"sln {solutionFile.FullName} add {clientProject.FullName} --in-root");
            if (addClientProjectResult.ExitCode != 0)
            {
                throw new Exception($"Error while adding client project to solution: {addClientProjectResult.Output}");
            }


            var addClientTestProjectResult = await dotnetTool.RunAsync("dotnet", $"sln {solutionFile.FullName} add {clientTestProject.FullName} --in-root");
            if (addClientTestProjectResult.ExitCode != 0)
            {
                throw new Exception($"Error while adding client test project to solution: {addClientTestProjectResult.Output}");
            }
        }
    }
}
