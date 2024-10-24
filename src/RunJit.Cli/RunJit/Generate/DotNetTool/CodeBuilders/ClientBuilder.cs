using System.Collections.Immutable;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal static class AddDotNetToolBuilderExtension
    {
        internal static void AddDotNetToolBuilder(this IServiceCollection services)
        {
            services.AddUsingsBuilder();
            services.AddParamaterBuilder();
            services.AddAssignExpressionBuilder();
            services.AddServiceRegistrationBuilder();
            services.AddPropertiesBuilder();

            services.AddSingletonIfNotExists<DotNetToolBuilder>();
        }
    }

    // What we are creating here:
    // 1. Correct using we need
    // 1. Service extensions with all the dependencies correctly registered
    // 2. The client class itself, with all correct injected dependencies
    //
    // using Microsoft.Extensions.DependencyInjection;
    // using Pulse.Sustainability.DotNetTool.Api.Accounts;    <-- UsingsBuilder
    //
    // namespace Pulse.Sustainability.DotNetTool
    // {
    //     public static class AddPulseSustainabilityDotNetToolExtension    <-- ServiceRegistrationBuilder
    //     {
    //         public static void AddPulseSustainabilityDotNetTool(this IServiceCollection services)
    //         {
    //             services.AddAccountsFacade();		
    //         }
    //     }
    //     
    //     public class PulseSustainabilityDotNetTool
    //     {  
    //         public PulseSustainabilityDotNetTool(AccountsFacade accountsFacade)  <-- ParameterBuilder
    //         {
    //             Accounts=accountsFacade;	 <-- AssignExpressionBuilder
    //         }
    //        
    //         public AccountsFacade Accounts { get; init; }  <-- PropertiesBuilder
    //     }
    // }
    internal class DotNetToolBuilder(UsingsBuilder usingsBuilder,
                                     ParameterBuilder parameterBuilder,
                                     AssignExpressionBuilder assignExpressionBuilder,
                                     ServiceRegistrationBuilder serviceRegistrationBuilder,
                                     PropertiesBuilder propertiesBuilder)
    {
        private readonly string _clientTemplate = EmbeddedFile.GetFileContentFrom("Pulse.Generate.DotNetTool.Templates.client.rps");

        public GeneratedDotNetTool BuildFor(IImmutableList<GeneratedFacade> facades,
                                            string projectName,
                                            string clientName)
        {
            var parameters = parameterBuilder.BuildFrom(facades);
            var serviceRegistrations = serviceRegistrationBuilder.BuildFrom(facades);
            var assignmentExpressions = assignExpressionBuilder.BuildFrom(facades);
            var properties = propertiesBuilder.BuildFrom(facades);
            var usings = usingsBuilder.BuildFrom(facades, projectName);

            var clientClass = _clientTemplate.Replace("$name$", clientName)
                                             .Replace("$serviceRegistrations$", serviceRegistrations)
                                             .Replace("$paramters$", parameters)
                                             .Replace("$assignmentExpressions$", assignmentExpressions)
                                             .Replace("$projectName$", projectName)
                                             .Replace("$clientName$", clientName)
                                             .Replace("$properties$", properties)
                                             .Replace("$usings$", usings)
                                             .Replace("$attributes$", string.Empty);

            return new GeneratedDotNetTool(facades, clientClass);
        }
    }
}
