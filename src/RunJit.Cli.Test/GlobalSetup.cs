using AspNetCore.Simple.Sdk.Authentication.Auth0;
using AspNetCore.Simple.Sdk.Mediator;
using DotNetTool.Service;
using Extensions.Pack;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RunJit.Cli.Test.SystemTest;

namespace RunJit.Cli.Test
{
    [TestClass]
    public class GlobalSetup
    {
        protected static IMediator Mediator { get; private set; } = null!;

        protected static DirectoryInfo WebApiFolder { get; private set; } = null!;

        protected static DirectoryInfo NugetFolder { get; private set; } = null!;

        protected static DirectoryInfo CodeCleanupFolder { get; private set; } = null!;

        protected static DirectoryInfo CodeRuleFolder { get; private set; } = null!;

        protected static IDotNetTool DotNetTool { get; private set; } = null!;

        protected static IServiceProvider Services { get; private set; } = null!;

        [AssemblyInitialize]
        public static async Task InitAsync(TestContext testContext)
        {
            var dotnetTool = DotNetToolFactory.Create();
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddMediator();
            serviceCollection.AddMediator(typeof(GetAuth0TokenFor));
            serviceCollection.AddSingleton(testContext);
            serviceCollection.AddSingleton<IDotNetTool>(dotnetTool);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            Mediator = serviceProvider.GetRequiredService<IMediator>();
            DotNetTool = DotNetToolFactory.Create();
            WebApiFolder = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "WebApi"));
            NugetFolder = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "Nuget"));
            CodeRuleFolder = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "CodeRules"));
            CodeCleanupFolder = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "CodeCleanup"));
            Services = serviceProvider;

            // Install components
            await Mediator.SendAsync(new InstallRequiredComponents()).ConfigureAwait(false);

            // 1. Cleanup anything before any test runs
            if (WebApiFolder.Exists)
            {
                WebApiFolder.Delete(true);
            }

            WebApiFolder.Create();

            if (NugetFolder.Exists)
            {
                NugetFolder.Delete(true);
            }

            if (CodeCleanupFolder.Exists)
            {
                foreach (var folder in CodeCleanupFolder.EnumerateDirectories("*", SearchOption.AllDirectories))
                {
                    if (folder.IsNotNull())
                    {
                        folder.Attributes = FileAttributes.Normal;

                        foreach (var info in folder.GetFileSystemInfos("*", SearchOption.AllDirectories))
                        {
                            info.Attributes = FileAttributes.Normal;
                        }

                        folder.Delete(true);
                    }
                }
            }
            else
            {
                CodeCleanupFolder.Create();
            }

            if (CodeRuleFolder.Exists)
            {
                foreach (var folder in CodeCleanupFolder.EnumerateDirectories("*", SearchOption.AllDirectories))
                {
                    if (folder.IsNotNull())
                    {
                        folder.Attributes = FileAttributes.Normal;

                        foreach (var info in folder.GetFileSystemInfos("*", SearchOption.AllDirectories))
                        {
                            info.Attributes = FileAttributes.Normal;
                        }

                        folder.Delete(true);
                    }
                }
            }

            CodeRuleFolder.Create();
        }
    }
}
