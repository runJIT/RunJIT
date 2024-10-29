using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal static class AddMsTestBaseClassBuilderExtension
    {
        internal static void AddMsTestBaseClassBuilder(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<MsTestBaseClassBuilder>();
        }
    }

    internal sealed class MsTestBaseClassBuilder
    {
        private readonly string _clientTemplate = EmbeddedFile.GetFileContentFrom("Pulse.Generate.DotNetTool.Templates.mstestbase.rps");

        public string BuildFor(string testProjectName,
                               string clientName)
        {
            var msTestBaseClass = _clientTemplate.Replace("$name$", clientName)
                                                 .Replace("$clientName$", clientName)
                                                 .Replace("$namespace$", testProjectName);

            return msTestBaseClass;
        }
    }
}
