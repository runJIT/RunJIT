using System.CommandLine;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using FileInfo = System.IO.FileInfo;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal static class AddDotNetToolOptionsBuilderExtension
    {
        internal static void AddDotNetToolOptionsBuilder(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<IDotNetToolGenOptionsBuilder, DotNetToolGenOptionsBuilder>();
        }
    }

    internal interface IDotNetToolGenOptionsBuilder
    {
        IEnumerable<Option> Build();
    }

    internal class DotNetToolGenOptionsBuilder : IDotNetToolGenOptionsBuilder
    {
        public IEnumerable<Option> Build()
        {
            yield return BuildUseVisualStudioOption();
            yield return Solution();
            yield return BuildMsBuild();
            yield return FromApi();
            yield return ToolName();
        }

        private Option BuildUseVisualStudioOption()
        {
            return new Option(new[] { "--use-visualstudio", "-uv" }, "Opens visual studio after generating the new dotnet tool. Only for Windows user !") { Required = false };
        }

        private Option BuildMsBuild()
        {
            return new Option(new[] { "--build", "-b" }, "Builds the target solution before creating the client") { Required = false };
        }

        private Option Solution()
        {
            return new Option(new[] { "--solution", "-s" }, @"File path to your backend solution where your api is implemented. Sample: D:\Projects\DotNetToolGen\DotNetToolGen.sln")
            {
                Required = false,
                Argument = new Argument<FileInfo>("solution") { Description = @"File path to your backend solution where your api is implemented. Sample: D:\Projects\DotNetToolGen\DotNetToolGen.sln" }
            };
        }

        private Option FromApi()
        {
            return new Option(new[] { "--from-api", "-fa" }, @"Generates an dotnet tool from an API to interact with the API")
            {
                Required = false,
                Argument = new Argument<bool>("fromApi")
                {
                    Description = @"Generates an dotnet tool from an API to interact with the API"
                }
            };
        }

        private Option ToolName()
        {
            return new Option(new[] { "--tool-name", "-tn" }, @"The name of the dotnet tool")
                   {
                       Required = true,
                       Argument = new Argument<string>("toolName")
                                  {
                                      Description = @"The name of the dotnet tool"
                                  }
                   };
        }
    }
}
