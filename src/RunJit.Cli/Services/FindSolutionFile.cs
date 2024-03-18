using Argument.Check;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.ErrorHandling;

namespace RunJit.Cli.Services
{
    public static class AddFindSolutionFileExtension
    {
        public static void AddFindSolutionFile(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<FindSolutionFile>();
        }
    }
    
    internal class FindSolutionFile
    {
        internal FileInfo Find(string solutionFile)
        {
            // 1. If we have a solution file we use it
            if (solutionFile.EndsWith(".sln"))
            {
                var fileInfo = new FileInfo(solutionFile);
                Throw.IfNotExists(fileInfo);

                return fileInfo;
            }
            
            // 2. If no value or . is used we are searching in the directory
            var currentDirectory = Environment.CurrentDirectory;
            var files = Directory.GetFiles(currentDirectory, "*.sln", SearchOption.AllDirectories);
            if (files.Length < 1)
            {
                throw new RunJitException($"Could not find a solution file in directory {currentDirectory}");
            }

            if (files.Length > 1)
            {
                throw new RunJitException($"Found more than one solution file in directory {currentDirectory}");
            }

            return new FileInfo(files[0]);
        }
    }
}
