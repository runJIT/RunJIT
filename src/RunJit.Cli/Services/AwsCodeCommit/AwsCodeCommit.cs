using System.Text;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Git;
using Process = DotNetTool.Service.Process;

namespace RunJit.Cli.AwsCodeCommit
{
    public static class AddAwsCodeCommitExtension
    {
        public static void AddAwsCodeCommit(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<IAwsCodeCommit, AwsCodeCommit>();
        }
    }

    public interface IAwsCodeCommit
    {
        Task CreatePullRequestAsync(string title,
                                    string description,
                                    string sourceBranchName,
                                    string targetBranchName = "master");

        Task CreatePullRequestAsync(string title,
                                    string description,
                                    string repositoryName,
                                    string sourceBranchName,
                                    string targetBranchName = "master");
    }

    internal class AwsCodeCommit(IConsoleService consoleService,
                                 IGitService git) : IAwsCodeCommit
    {
        public async Task CreatePullRequestAsync(string title,
                                                 string description,
                                                 string sourceBranchName,
                                                 string targetBranchName = "master")
        {
            var gitOrigin = await git.GetOriginAsync().ConfigureAwait(false);
            var repoName = gitOrigin.Split("//").Last();

            await CreatePullRequestAsync(title,
                                         description,
                                         repoName,
                                         sourceBranchName,
                                         targetBranchName).ConfigureAwait(false);
        }

        public async Task CreatePullRequestAsync(string title,
                                                 string description,
                                                 string repositoryName,
                                                 string sourceBranchName,
                                                 string targetBranchName = "master")
        {
            consoleService.WriteInfo("Create AWS Code commit PR");
            consoleService.WriteInfo($@"aws codecommit create-pull-request --title ""{title}"" --description ""{description}"" --targets repositoryName=""{repositoryName},sourceReference={sourceBranchName},destinationReference={targetBranchName}""");

            var stringBuilder = new StringBuilder();
            var pullRequestResponse = Process.StartProcess("aws", $@"codecommit create-pull-request --title ""{title}"" --description ""{description}"" --targets repositoryName=""{repositoryName},sourceReference={sourceBranchName},destinationReference={targetBranchName}""");
            await pullRequestResponse.WaitForExitAsync().ConfigureAwait(false);

            var output = stringBuilder.ToString();

            // var pullRequestResponse = await donDotNetTool.RunAsync("aws", $@"codecommit create-pull-request --title ""{title}"" --description ""{description}"" --targets repositoryName=""{repositoryName},sourceReference={sourceBranchName},destinationReference={targetBranchName}""").ConfigureAwait(false);
            if (pullRequestResponse.ExitCode != 0 &&
                pullRequestResponse.ExitCode != 255) // 255 is creating PR successfully :/ strange
            {
                consoleService.WriteError($"Failed to create pull request. Exit code: {pullRequestResponse.ExitCode}. Message: {output}");
            }

            consoleService.WriteSuccess("AWS Code Commit pull request successfully created");
        }
    }
}
