using System.IO.Compression;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using Argument.Check;
using AspNetCore.Simple.Sdk.Mediator;
using DotNetTool.Service;
using Extensions.Pack;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Api.Client;
using RunJit.Cli.Auth0;
using RunJit.Cli.ErrorHandling;
using RunJit.Cli.Net;
using RunJit.Cli.RunJit.New.Lambda.Models;

namespace RunJit.Cli.RunJit.New.Lambda
{
    public static class AddLambdaServiceExtension
    {
        public static void AddLambdaService(this IServiceCollection services)
        {
            services.AddConsoleService();
            services.AddLambdaParameters();
            services.AddTemplateService();
            services.AddSingletonIfNotExists<ILambdaService, LambdaService>();
        }
    }

    internal interface ILambdaService
    {
        Task HandleAsync(LambdaParameters parameters);
    }

    internal partial class LambdaService(TemplateService templateService, 
                                         IConsoleService consoleService,
                                         IRunJitApiClientFactory runJitApiClientFactory,
                                         IMediator mediator,
                                         IHttpClientFactory httpClientFactory,
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

            // 3. Download template
            var combine = Path.Combine(parameters.Solution.Directory!.FullName, Guid.NewGuid().ToString().ToLowerInvariant());
            var tempFolder = new DirectoryInfo(combine);
            
            var auth = await mediator.SendAsync(new GetTokenByStorageCache()).ConfigureAwait(false);
            var httpClient = httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(auth.TokenType, auth.Token);
            var rRunJitApiClient = runJitApiClientFactory.CreateFrom(httpClient);

            var codeRuleAsFileStream = await rRunJitApiClient.CodeRules.V1.ExportCodeRulesAsync().ConfigureAwait(false);
            using var zipArchive = new ZipArchive(codeRuleAsFileStream.FileStream, ZipArchiveMode.Read);
            zipArchive.ExtractToDirectory(tempFolder.FullName);
            
            // 3. replace placeholders
            templateService.RenameAllIn(tempFolder, lambdaInfos);

            // Move all folder into real target solution folder
            foreach (var directory in tempFolder.EnumerateDirectories())
            {
                var destDirName = new DirectoryInfo(Path.Combine(parameters.Solution.Directory.FullName, directory.Name));
                Directory.Move(directory.FullName, destDirName.FullName);

                var csprojFiles = destDirName.EnumerateFiles("*.csproj", SearchOption.TopDirectoryOnly);
                foreach (var csproj in csprojFiles)
                {
                    await dotNet.AddProjectToSolutionAsync(parameters.Solution, csproj).ConfigureAwait(false);
                }
                
            }

            consoleService.WriteSuccess($"Lambda: '{parameters.LambdaName}' successful created");

            consoleService.WriteSuccess(EmbeddedFile.GetFileContentFrom("Logo.Logo.txt"));
        }

        private LambdaParameters PreparedParameters(LambdaParameters parameters)
        {
            return new LambdaParameters(parameters.Solution, parameters.ModuleName.ToLower(), parameters.FunctionName.FirstCharToUpper(), parameters.LambdaName.ToLower(),  parameters.GitRepos, parameters.Branch, parameters.WorkingDirectory);
        }

        private static async Task<DotNetTool.Service.DotNetTool> IncludeIntoSolutionAsync(string solutionFullName, DirectoryInfo solutionDirectory, string projectName)
        {
            var projectFiles = solutionDirectory.EnumerateFiles("*.csproj", searchOption: SearchOption.AllDirectories).Where(file => file.Name.Contains(projectName)).ToList();
            var dotNetTool = DotNetToolFactory.Create();

            foreach (var project in projectFiles)
            {
                await dotNetTool.RunAsync("dotnet", $"sln {solutionFullName} add {project.FullName}").ConfigureAwait(false);
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
