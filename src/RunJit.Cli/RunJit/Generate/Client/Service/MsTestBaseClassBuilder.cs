using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.Generate.Client
{
    public static class AddMsTestBaseClassBuilderExtension
    {
        public static void AddMsTestBaseClassBuilder(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<MsTestBaseClassBuilder>();
        }
    }

    internal class MsTestBaseClassBuilder
    {
        private readonly string _clientTemplate = EmbeddedFile.GetFileContentFrom("RunJit.Generate.Client.Templates.mstestbase.rps");

        public string BuildFor(string testProjectName, string clientName)
        {
            var msTestBaseClass = _clientTemplate.Replace("$name$", clientName)
                                                 .Replace("$clientName$", clientName)
                                                 .Replace("$namespace$", testProjectName);

            return msTestBaseClass;
        }
    }
}
