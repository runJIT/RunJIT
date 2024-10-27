using Argument.Check;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.Services
{
    public static class AddFindSourceFolderExtension
    {
        public static void AddFindSourceFolder(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<FindSourceFolder>();
        }
    }

    internal sealed class FindSourceFolder
    {
        internal DirectoryInfo GetTargetSourceFolder(FileInfo solutionFileInfo)
        {
            Throw.IfNotExists(solutionFileInfo);

            // 1. Looking for existing .csproj file
            var findcsproj = solutionFileInfo.Directory!.EnumerateFiles("*.csproj", SearchOption.AllDirectories).FirstOrDefault();

            // 2. If not found we create a src directory
            if (findcsproj.IsNotNull())
            {
                return findcsproj.Directory!;
            }

            var sourceDirectory = new DirectoryInfo(Path.Combine(solutionFileInfo.Directory.FullName, "src"));
            sourceDirectory.Create();

            return sourceDirectory;
        }
    }
}
