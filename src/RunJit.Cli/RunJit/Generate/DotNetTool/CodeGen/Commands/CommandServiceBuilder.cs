﻿using Argument.Check;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using Solution.Parser.CSharp;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    public static class AddCommandServiceBuilderExtension
    {
        public static void AddCommandServiceBuilder(this IServiceCollection services)
        {
            services.AddCommandMethodBuilderBuilder();

            services.AddSingletonIfNotExists<CommandServiceBuilder>();
        }
    }

    internal sealed class CommandServiceBuilder(CommandMethodBuilder commandMethodBuilder)
    {
        private const string Template = """
                                        using Extensions.Pack;
                                        using Microsoft.Extensions.DependencyInjection;
                                        
                                        namespace $namespace$
                                        {    
                                            internal static class Add$command-name$HandlerExtension
                                            {
                                                internal static void Add$command-name$Handler(this IServiceCollection services, IConfiguration configuration)
                                                {
                                                    services.Add$dotNetToolName$HttpClientFactory(configuration);
                                                
                                                    services.AddSingletonIfNotExists<$command-name$Handler>();
                                                }
                                            }
                                        
                                            internal sealed class $command-name$Handler$dependencies$
                                            {       
                                                public Task HandleAsync($command-name$Parameters parameters)
                                                {
                                                    $methodBody$
                                                }
                                            }
                                        }
                                        """;

        public string Build(string project,
                            CommandInfo commandInfo,
                            string nameSpace,
                            DotNetToolName dotNetToolName)
        {
            Throw.IfNullOrWhiteSpace(project);
            Throw.IfNullOrWhiteSpace(nameSpace);

            var dependencies = commandInfo.EndpointInfo.IsNull() ? string.Empty : $"({dotNetToolName.NormalizedName}HttpClientFactory {dotNetToolName.NormalizedName.FirstCharToLower()}HttpClientFactory)";
            var methodBody = commandInfo.EndpointInfo.IsNull() ? "throw new NotImplementedException();" : commandMethodBuilder.BuildFor(commandInfo.EndpointInfo);

            var newTemplate = Template.Replace("$command-name$", commandInfo.NormalizedName)
                                      .Replace("$namespace$", nameSpace)
                                      .Replace("$project-name$", project)
                                      .Replace("$methodBody$", methodBody)
                                      .Replace("$dependencies$", dependencies)
                                      .Replace("$dotNetToolName$", dotNetToolName.NormalizedName);

            return newTemplate.FormatSyntaxTree();
        }
    }
}
