using System.Collections.Immutable;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.ErrorHandling;
using RunJit.Cli.Git;
using RunJit.Cli.Net;
using RunJit.Cli.RunJit.Update.Net;
using RunJit.Cli.Services;
using Solution.Parser.CSharp;
using Solution.Parser.Solution;

namespace RunJit.Cli.RunJit.Check.Backend.Builds
{
    internal static class AddUpdateLocalSolutionFileExtension
    {
        internal static void AddUpdateLocalSolutionFile(this IServiceCollection services)
        {
            services.AddConsoleService();
            services.AddGitService();
            services.AddDotNet();
            services.AddDotNetService();
            // services.AddCheckBackendBuildsPackageService();
            services.AddFindServiceImplementations();
            services.AddServiceRegistrationFixture();
            services.AddFindSolutionFile();

            services.AddSingletonIfNotExists<ICheckBackendBuildsStrategy, UpdateLocalSolutionFile>();
        }
    }

    internal class UpdateLocalSolutionFile(IConsoleService consoleService,
                                           FindServiceImplementations findServiceImplementations,
                                           ServiceRegistrationFixture serviceRegistrationFixture,
                                           FindSolutionFile findSolutionFile) : ICheckBackendBuildsStrategy
    {
        public bool CanHandle(CheckBackendBuildsParameters parameters)
        {
            return parameters.SolutionFile.IsNotNullOrWhiteSpace();
        }

        public async Task HandleAsync(CheckBackendBuildsParameters parameters)
        {
            // 0. Check that precondition is met
            if (CanHandle(parameters).IsFalse())
            {
                throw new RunJitException($"Please call {nameof(ICheckBackendBuildsStrategy.CanHandle)} before call {nameof(ICheckBackendBuildsStrategy.HandleAsync)}");
            }

            // 1. Check if solution file is the file or directory
            //    if it is null or whitespace we check current directory
            var solutionFileInfo = findSolutionFile.Find(parameters.SolutionFile);

            var solutionFile = new SolutionFileInfo(solutionFileInfo.FullName).Parse();
            var productiveCodeSyntaxTreesToAnaylze = solutionFile.ProductiveProjects.SelectMany(p => p.CSharpFileInfos)
                                                                 .Select(p => p.Parse()).ToImmutableList();

            // 1. Detect all service registrations
            var serviceRegistrationInfos = findServiceImplementations.FindAll(productiveCodeSyntaxTreesToAnaylze);

            // 2. Fix all findings
            await serviceRegistrationFixture.FixAllAsync(serviceRegistrationInfos, productiveCodeSyntaxTreesToAnaylze).ConfigureAwait(false);

            consoleService.WriteSuccess($"Service registrations in: {solutionFileInfo.FullName} successfully fixed");
        }
    }


    public static class AddFindServiceImplementationsExtension
    {
        public static void AddFindServiceImplementations(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<FindServiceImplementations>();
        }
    }

    internal class FindServiceImplementations
    {
        internal IImmutableList<ServiceRegistrationInfo> FindAll(ImmutableList<CSharpSyntaxTree> syntaxTrees)
        {
            var serviceRegistrationInfos = FindServices(syntaxTrees).ToImmutableList();

            return serviceRegistrationInfos;


            IEnumerable<ServiceRegistrationInfo> FindServices(ImmutableList<CSharpSyntaxTree> syntaxTrees)
            {
                foreach (var cSharpSyntaxTree in syntaxTrees)
                {
                    var services = cSharpSyntaxTree.Classes.Where(@class => @class.Modifiers.Any(m => m is Modifier.Static or Modifier.Abstract).IsFalse() &&
                                                                            @class.Methods.Any() &&
                                                                            @class.Attributes.Any(a => a.Name.Contains("ApiController")).IsFalse() &&
                                                                            @class.Name.EndsWith("Handler").IsFalse() &&
                                                                            @class.BaseTypes.Any(b => b.TypeName.StartsWith("IComparer<")).IsFalse()&&
                                                                            @class.Name != nameof(Startup) &&
                                                                            @class.Name != "App").ToList();

                    foreach (var service in services)
                    {
                        var registrationClass = cSharpSyntaxTree.Classes.FirstOrDefault(@class => @class.Name.Contains($"{service.Name}Extension"));
                        yield return new(service, registrationClass, cSharpSyntaxTree);
                    }
                }
            }
        }
    }

    public static class AddServiceRegistrationFixtureExtension
    {
        public static void AddServiceRegistrationFixture(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<ServiceRegistrationFixture>();
        }
    }

    internal class ServiceRegistrationFixture
    {
        private const string Template = @"
    internal static class Add$className$Extension
    {
        internal static void Add$className$(this IServiceCollection services)
        {   
$registrations$
        }
    }
";



        internal async Task FixAllAsync(IImmutableList<ServiceRegistrationInfo> serviceRegistrationInfos,
                                        IImmutableList<CSharpSyntaxTree> allSyntaxTrees)
        {
            var whiteList = new string[] { "App", "Program", "Function", "Startup" };

            // What we need to do?
            // We have to find all kind of classes, settings, options, middlewares, services, etc.
            // Then we have to check if each of them has a registration class
            // Then we have to check if each of them register all their dependencies
            // Then we have to check if each of them register themselves
            // Then we have to check if the service registration class is in the same file as the service class

            // We need fixture for:
            // services
            // options
            // middleware  -> Add + Use
            // mediator commands/queries
            // mediator behaviors
            foreach (var serviceRegistrationInfo in serviceRegistrationInfos)
            {
                if (serviceRegistrationInfo.Registration.IsNotNull())
                {
                    continue;
                }

                if (whiteList.Any(name => serviceRegistrationInfo.Class.Name == name))
                {
                    continue;
                }

                var interfaceImplementation = serviceRegistrationInfo.Class.BaseTypes.FirstOrDefault(b => b.TypeName.StartsWith("I"));

                var parameters = serviceRegistrationInfo.Class.Parameters.Select(p => p.Type);
                var dependencyRegistrations = parameters.Select(d => $"            services.Add{d.TrimStart('I')}();").Flatten(Environment.NewLine);
                var typeRegistration = interfaceImplementation.IsNull() ? serviceRegistrationInfo.Class.Name : $"{interfaceImplementation.TypeName}, {serviceRegistrationInfo.Class.Name}";

                var registrations = dependencyRegistrations.IsNullOrWhiteSpace() ? $"            services.AddSingletonIfNotExists<{typeRegistration}>();" : $"{dependencyRegistrations}{Environment.NewLine}{Environment.NewLine}            services.AddSingletonIfNotExists<{typeRegistration}>();";

                var registrationExtension = Template.Replace("$className$", serviceRegistrationInfo.Class.Name)
                                                    .Replace("$registrations$", registrations);

                
                // 0. Check if extensions exist anywhere else
                var extensionClass = allSyntaxTrees.Where(tree => tree.Classes.Any(c => c.Name.Contains($"Add{serviceRegistrationInfo.Class.Name}")));
                if (extensionClass.Any())
                {
                    foreach (var cSharpSyntaxTree in extensionClass)
                    {
                        // If the file contains only that extensions kill it
                        if (cSharpSyntaxTree.Classes.Count == 1)
                        {
                            File.Delete(cSharpSyntaxTree.Classes[0].FilePath);
                        }
                        else
                        {
                            var classToRemove = cSharpSyntaxTree.Classes.FirstOrDefault(c => c.Name.Contains($"Add{serviceRegistrationInfo.Class.Name}"));
                            var extensionClassToRemove = await File.ReadAllTextAsync(serviceRegistrationInfo.Class.FilePath).ConfigureAwait(false);
                            var newClassContent = extensionClassToRemove.Replace(classToRemove!.SyntaxTree, string.Empty);
                            await File.WriteAllTextAsync(serviceRegistrationInfo.Class.FilePath, newClassContent).ConfigureAwait(false);
                        }
                    }
                }
                
                
                // 1. Check if registration class exists
                var fileContent = await File.ReadAllTextAsync(serviceRegistrationInfo.Class.FilePath).ConfigureAwait(false);

                var newSyntaxTree = $"{registrationExtension}{Environment.NewLine}{serviceRegistrationInfo.Class.SyntaxTree}";

                var newFileContent = fileContent.Replace(serviceRegistrationInfo.Class.SyntaxTree, newSyntaxTree);

                var syntaxTreeUsings = serviceRegistrationInfo.syntaxTree.Usings.Select(item => item.Value).ToImmutableList();
                var diUsingExists = syntaxTreeUsings.Contains("Microsoft.Extensions.DependencyInjection");
                if (diUsingExists.IsFalse())
                {
                    var newusings = syntaxTreeUsings.Add("Microsoft.Extensions.DependencyInjection");

                    if (syntaxTreeUsings.Contains("Extensions.Pack").IsFalse())
                    {
                        newusings = newusings.Add("Extensions.Pack");
                    }
                    
                    var oldValue = syntaxTreeUsings.Select(v => $"using {v};").Flatten(Environment.NewLine);
                    var newValue = newusings.Select(v => $"using {v};").Flatten(Environment.NewLine);

                    newFileContent = newFileContent.Replace(oldValue, newValue);
                }
                
                await File.WriteAllTextAsync(serviceRegistrationInfo.Class.FilePath, newFileContent).ConfigureAwait(false);
            }
        }
    }

    internal record ServiceRegistrationInfo(Class Class, Class? Registration, CSharpSyntaxTree syntaxTree);
}
