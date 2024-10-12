using System.Text.RegularExpressions;
using Argument.Check;
using DotNetTool.Service;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.ErrorHandling;
using RunJit.Cli.Services;
using RunJit.Cli.Services.Net;

namespace RunJit.Cli.RunJit.New.Lambda
{
    public static class AddLambdaServiceExtension
    {
        public static void AddLambdaService(this IServiceCollection services)
        {
            services.AddConsoleService();
            services.AddLambdaParameters();
            services.AddTemplateExtractor();
            services.AddTemplateService();
            services.AddSingletonIfNotExists<ILambdaService, LambdaService>();
        }
    }

    internal interface ILambdaService
    {
        Task HandleAsync(LambdaParameters parameters);
    }

    internal partial class LambdaService(TemplateExtractor templateExtractor,
                                         TemplateService templateService,
                                         IConsoleService consoleService,
                                         IDotNet dotNet) : ILambdaService
    {
        public async Task HandleAsync(LambdaParameters parameters)
        {
            // 1. validate parameters
            ValidateParameters(parameters);
            parameters = PreparedParameters(parameters);
            Throw.IfNull(parameters.Solution.Directory);

            // 2. collect information
            var projectName = ExtractProjectName(parameters);
            var lambdaInfos = new LambdaInfos(parameters, projectName);

            // 2. extract lambda template
            await templateExtractor.ExtractToAsync(parameters.Solution.Directory).ConfigureAwait(false);

            // 3. replace placeholders
            templateService.RenameAllIn(parameters.Solution.Directory, lambdaInfos);

            // 4. include generated projects into the solution
            var dotNetTool = await IncludeIntoSolutionAsync(parameters.Solution, parameters.Solution.Directory, projectName).ConfigureAwait(false);

            // 5. Update nuget packages
            await dotNetTool.RunAsync("dotnet", $"restore {parameters.Solution.FullName}").ConfigureAwait(false);

            consoleService.WriteSuccess($"Pulse lambda: '{parameters.LambdaName}' successful created");

            consoleService.WriteSuccess(EmbeddedFile.GetFileContentFrom("Logo.RunJit.txt"));
        }

        private LambdaParameters PreparedParameters(LambdaParameters parameters)
        {
            return new LambdaParameters(parameters.Solution, parameters.ModuleName.ToLower(), parameters.FunctionName.FirstCharToUpper(),
                                        parameters.LambdaName.ToLower());
        }

        private async Task<DotNetTool.Service.DotNetTool> IncludeIntoSolutionAsync(FileInfo solutionFile,
                                                                                   DirectoryInfo solutionDirectory,
                                                                                   string projectName)
        {
            var projectFiles = solutionDirectory.EnumerateFiles("*.csproj", SearchOption.AllDirectories).Where(file => file.Name.Contains(projectName)).ToList();
            var dotNetTool = DotNetToolFactory.Create();

            foreach (var project in projectFiles)
            {
                await dotNet.AddProjectToSolutionAsync(solutionFile, project).ConfigureAwait(false);
            }

            return dotNetTool;
        }

        private static string ExtractProjectName(LambdaParameters parameters)
        {
            return parameters.LambdaName.Split("-").Select(word => word.FirstCharToUpper()).Flatten(".");
        }

        private void ValidateParameters(LambdaParameters parameters)
        {
            if (parameters.Solution.NotExists())
            {
                throw new RunJitException($"The provided solution file: {parameters.Solution.FullName} does not exist.");
            }

            if (AlphanumericWithMinus().IsMatch(parameters.ModuleName).IsFalse())
            {
                throw new RunJitException("ModuleName should contain no special characters other than '-'. " +
                                          "\nExample: 'core'");
            }

            if (AlphanumericWithStartingLetter().IsMatch(parameters.FunctionName).IsFalse())
            {
                throw new RunJitException("FunctionName should be alphanumeric and not begin with a number. " +
                                          "\nExample: 'CallGpt'");
            }

            if (AlphanumericWithMinus().IsMatch(parameters.LambdaName).IsFalse())
            {
                throw new RunJitException("LambdaName should contain no special characters other than '-'. " +
                                          "\nExample: 'analytics-gpt-chat'");
            }
        }

        [GeneratedRegex("^[a-zA-Z0-9-]+$")]
        private static partial Regex AlphanumericWithMinus();

        [GeneratedRegex("^[a-zA-Z][a-zA-Z0-9]*$")]
        private static partial Regex AlphanumericWithStartingLetter();
    }
}
