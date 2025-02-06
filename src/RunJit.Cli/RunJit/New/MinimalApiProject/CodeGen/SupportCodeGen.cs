using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;
using Solution.Parser.Solution;

namespace RunJit.Cli.New.MinimalApiProject.CodeGen
{
    internal static class AddSupportCodeGenExtension
    {
        internal static void AddSupportCodeGen(this IServiceCollection services)
        {
            services.AddConsoleService();
            services.AddNamespaceProvider();

            services.AddSingletonIfNotExists<IMinimalApiProjectRootLevelCodeGen, SupportCodeGen>();
        }
    }

    internal sealed class SupportCodeGen(ConsoleService consoleService) : IMinimalApiProjectRootLevelCodeGen
    {
        private const string Template = """
                                        # Support
                                        
                                        Welcome to the Support page. If you need assistance, we’re here to help! Below you’ll find a list of resources, frequently asked questions, and ways to contact us.
                                        
                                        ---
                                        
                                        ## 🔧 Troubleshooting
                                        
                                        ### Common Issues
                                        
                                        1. **Issue 1**: Setup DynamoDB locally 
                                            - **Solution**: Follow the Step-by-Step guide to setup a daynamo db locally.
                                        
                                        ---
                                        
                                        ## 📚 Frequently Asked Questions (FAQ)
                                        
                                        ### Q: How do I contact support?
                                        **A**: You can create a new issue on GitHub.
                                        
                                        ---
                                        
                                        ## 📝 Submit a Support Ticket
                                        
                                        If you couldn't find the answer you're looking for, please submit a support ticket below:
                                        
                                        [Submit a Support Ticket](https://github.siemens.cloud/sdc/siemens-data-cloud-core/issues)
                                        
                                        ---
                                        
                                        ## 📄 Additional Resources
                                        
                                        - Take a look into out [Wiki](https://github.siemens.cloud/sdc/siemens-data-cloud-core/wiki)
                                        """;


        public async Task GenerateAsync(SolutionFile solutionFile,
                                        MinimalApiProjectInfos minimalApiProjectInfos)
        {
            // 1 Setup file name
            var file = Path.Combine(solutionFile.SolutionFileInfo.Value.Directory!.FullName, "SUPPORT.md");


            await File.WriteAllTextAsync(file, Template).ConfigureAwait(false);

            // 3. Print success message
            consoleService.WriteSuccess($"Successfully created {file}");
        }
    }
}
