using System.CommandLine;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Generate.CustomEndpoint
{
    internal static class AddGenerateCustomEndpointOptionsBuilderExtension
    {
        internal static void AddGenerateCustomEndpointOptionsBuilder(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<IGenerateEndpointOptionsBuilder, GenerateCustomEndpointOptionsBuilder>();
        }
    }

    internal interface IGenerateEndpointOptionsBuilder
    {
        IEnumerable<Option> Build();
    }

    internal sealed class GenerateCustomEndpointOptionsBuilder : IGenerateEndpointOptionsBuilder
    {
        public IEnumerable<Option> Build()
        {
            yield return BuildDirectoryInfoOption();
            yield return BuildEndpointInfos();
            yield return BuildOverwriteExistingCode();
        }

        private Option BuildDirectoryInfoOption()
        {
            return new Option(new[] { "--target-folder", "-tf" }, "The target folder in which the code should be generated")
                   {
                       Required = true,
                       Argument = new Argument<DirectoryInfo>("targetFolder") { Description = "The target folder in which the code should be generated" }
                   };
        }

        private Option BuildEndpointInfos()
        {
            return new Option(new[] { "--endpoint-data", "-ed" }, "The json structure which contains all needed data to create your endpoint")
                   {
                       Required = false,
                       Argument = new Argument<string>("endpointData") { Description = "The json structure which contains all needed data to create your endpoint" }
                   };
        }

        private Option BuildOverwriteExistingCode()
        {
            return new Option(new[] { "--overwrite-code", "-oc" }, "Overwrites the code already if it exist")
                   {
                       Required = false,
                       Argument = new Argument<string>("overwriteCode") { Description = "Overwrites the code already if it exist" }
                   };
        }
    }

    //internal sealed record EndpointData
    //{
    //    public string DomainName { get; init; }
    //    public string Version { get; init; }
    //    public string HttpMethod { get; init; }
    //    public string Sql { get; init; }

    //    public string Endpoint { get; init; }
    //    public string IdName { get; init; }
    //    public string ModelName { get; init; }
    //    public string DbSecret { get; init; }
    //    public string FromSql { get; init; }
    //    public bool WithCaching { get; init; }
    //}
}
