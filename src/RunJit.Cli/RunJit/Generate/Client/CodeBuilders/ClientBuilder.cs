using System.Collections.Immutable;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Generate.Client
{
    internal static class AddClientBuilderExtension
    {
        internal static void AddClientBuilder(this IServiceCollection services)
        {
            services.AddUsingsBuilder();
            services.AddParamaterBuilder();
            services.AddAssignExpressionBuilder();
            services.AddServiceRegistrationBuilder();
            services.AddPropertiesBuilder();

            services.AddSingletonIfNotExists<ClientBuilder>();
        }
    }

    // What we are creating here:
    // 1. Correct using we need
    // 1. Service extensions with all the dependencies correctly registered
    // 2. The client class itself, with all correct injected dependencies
    //
    // using Microsoft.Extensions.DependencyInjection;
    // using Api.Accounts;    <-- UsingsBuilder
    //
    // namespace Api.Client
    // {
    //     public static class AddPulseSustainabilityClientExtension    <-- ServiceRegistrationBuilder
    //     {
    //         public static void AddPulseSustainabilityClient(this IServiceCollection services)
    //         {
    //             services.AddAccountsFacade();		
    //         }
    //     }
    //     
    //     public class PulseSustainabilityClient
    //     {  
    //         public PulseSustainabilityClient(AccountsFacade accountsFacade)  <-- ParameterBuilder
    //         {
    //             Accounts=accountsFacade;	 <-- AssignExpressionBuilder
    //         }
    //        
    //         public AccountsFacade Accounts { get; init; }  <-- PropertiesBuilder
    //     }
    // }
    internal class ClientBuilder(UsingsBuilder usingsBuilder,
                                 ParameterBuilder parameterBuilder,
                                 AssignExpressionBuilder assignExpressionBuilder,
                                 ServiceRegistrationBuilder serviceRegistrationBuilder,
                                 PropertiesBuilder propertiesBuilder)
    {
        private readonly string _clientTemplate = EmbeddedFile.GetFileContentFrom("RunJit.Generate.Client.Templates.client.rps");

        public GeneratedClient BuildFor(IImmutableList<GeneratedFacade> facades, string projectName, string clientName)
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

            return new GeneratedClient(facades, clientClass);
        }
    }
}
