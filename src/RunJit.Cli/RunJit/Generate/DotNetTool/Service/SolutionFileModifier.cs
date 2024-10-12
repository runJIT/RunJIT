using Argument.Check;
using DotNetTool.Service;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
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

            var clientProject = solutionFile.Directory.EnumerateFiles($"{solutionFile.NameWithoutExtension()}.DotNetTool.csproj", SearchOption.AllDirectories).FirstOrDefault();
            var clientTestProject = solutionFile.Directory.EnumerateFiles($"{solutionFile.NameWithoutExtension()}.DotNetTool.Test.csproj", SearchOption.AllDirectories).FirstOrDefault();
            Throw.IfNull(clientProject);
            Throw.IfNull(clientTestProject);

            var dotnetTool = DotNetToolFactory.Create();
            var addDotNetToolProjectResult = await dotnetTool.RunAsync("dotnet", $"sln {solutionFile.FullName} add {clientProject.FullName} --in-root");
            if (addDotNetToolProjectResult.ExitCode != 0)
            {
                throw new Exception($"Error while adding client project to solution: {addDotNetToolProjectResult.Output}");
            }


            var addDotNetToolTestProjectResult = await dotnetTool.RunAsync("dotnet", $"sln {solutionFile.FullName} add {clientTestProject.FullName} --in-root");
            if (addDotNetToolTestProjectResult.ExitCode != 0)
            {
                throw new Exception($"Error while adding client test project to solution: {addDotNetToolTestProjectResult.Output}");
            }
        }
    }
}
