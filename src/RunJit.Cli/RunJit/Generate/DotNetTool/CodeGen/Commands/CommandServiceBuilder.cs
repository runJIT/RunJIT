using Argument.Check;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using Solution.Parser.CSharp;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    public static class AddCommandServiceBuilderExtension
    {
        public static void AddCommandServiceBuilder(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<CommandServiceBuilder>();
        }
    }

    internal sealed class CommandServiceBuilder
    {
        private const string Template = """
                                        using Extensions.Pack;
                                        using Microsoft.Extensions.DependencyInjection;
                                        
                                        namespace $namespace$
                                        {    
                                            internal static class Add$command-name$ServiceExtension
                                            {
                                                internal static void Add$command-name$Service(this IServiceCollection services)
                                                {
                                                    services.AddSingletonIfNotExists<$command-name$Service>();
                                                }
                                            }
                                        
                                            internal sealed class $command-name$Service
                                            {       
                                                public Task HandleAsync($command-name$Parameters parameters)
                                                {
                                                    throw new NotImplementedException();
                                                }
                                            }
                                        }
                                        """;

        public string Build(string project,
                            CommandInfo parameterInfo,
                            string nameSpace)
        {
            Throw.IfNullOrWhiteSpace(project);
            Throw.IfNullOrWhiteSpace(nameSpace);

            var newTemplate = Template.Replace("$command-name$", parameterInfo.NormalizedName)
                                      .Replace("$namespace$", nameSpace)
                                      .Replace("$project-name$", project);

            return newTemplate.FormatSyntaxTree();
        }
    }
}
