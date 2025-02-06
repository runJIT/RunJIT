using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;
using Solution.Parser.Solution;

namespace RunJit.Cli.New.MinimalApiProject.CodeGen
{
    internal static class AddSecurityCodeGenExtension
    {
        internal static void AddSecurityCodeGen(this IServiceCollection services)
        {
            services.AddConsoleService();
            services.AddNamespaceProvider();

            services.AddSingletonIfNotExists<IMinimalApiProjectRootLevelCodeGen, SecurityCodeGen>();
        }
    }

    internal sealed class SecurityCodeGen(ConsoleService consoleService) : IMinimalApiProjectRootLevelCodeGen
    {
        private const string Template = """
                                        # Security Policy
                                        
                                        Siemens takes the security of its code seriously. If you think you have found a security vulnerability,
                                        please read the next sections and follow the instructions to report your finding.
                                        
                                        ## Scope of this policy
                                        
                                        This is the default security policy for all repositories within the `siemens` organization on GitHub.com.
                                        
                                        It does not apply for reposities that have their own security policy.
                                        It also does not apply for forks where you should follow the upstream policy instead.
                                        
                                        If you are unsure whether the policy applies feel free to reach out via the channels mentioned below and we'll be happy to help.
                                        
                                        ## Reporting a Vulnerability
                                        
                                        Please DO NOT report any potential security vulnerability via a public channel (mailing list, GitHub issue, etc.).
                                        Instead, [report the vulnerability privately via GitHub](https://docs.github.com/en/code-security/security-advisories/guidance-on-reporting-and-writing/privately-reporting-a-security-vulnerability)
                                        (if enabled for the repository) or [contact us via email](mailto:opensource@siemens.com).
                                        
                                        Please provide a detailed description of the issue, the steps to reproduce it, the affected version(s) and, if already available,
                                        a proposal for a fix. You should receive a response within 5 working days. If for some reason you do not, please follow up via email to ensure we received your original message.
                                        
                                        If we confirm the issue as a vulnerability, we will publish an advisory (e.g. on GitHub) and give credits for your report if desired. We follow the [coordinated vulnerability disclosure](https://vuls.cert.org/confluence/display/CVD) model and will define an appropriate disclosure timeline together with you.
                                        
                                        """;


        public async Task GenerateAsync(SolutionFile solutionFile,
                                        MinimalApiProjectInfos minimalApiProjectInfos)
        {
            // 1 Setup file name
            var file = Path.Combine(solutionFile.SolutionFileInfo.Value.Directory!.FullName, "SECURITY.md");


            await File.WriteAllTextAsync(file, Template).ConfigureAwait(false);

            // 3. Print success message
            consoleService.WriteSuccess($"Successfully created {file}");
        }
    }
}
