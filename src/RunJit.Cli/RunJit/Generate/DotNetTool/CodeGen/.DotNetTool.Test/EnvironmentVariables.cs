using System.Xml.Linq;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;
using Solution.Parser.CSharp;
using Solution.Parser.Project;

namespace RunJit.Cli.Generate.DotNetTool.DotNetTool.Test
{
    internal static class AddEnvironmentVariablesCodeGenExtension
    {
        internal static void AddEnvironmentVariablesCodeGen(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<IDotNetToolTestSpecificCodeGen, EnvironmentVariablesCodeGen>();
        }
    }

    internal sealed class EnvironmentVariablesCodeGen(ConsoleService consoleService) : IDotNetToolTestSpecificCodeGen
    {
        private const string Template = """
                                        {
                                          "ASPNETCORE_ENVIRONMENT": "Development",
                                          "Logging__LogLevel__Default": "Information",
                                          "Logging__LogLevel__Microsoft.AspNetCore": "Warning",
                                          "AllowedHosts": "*",
                                          "AWS__Region": "eu-west-1",
                                          "AWS__Profile": "sdcdev",
                                          "AWS__DynamoDbServiceUrl": "http://localhost:8001",
                                          "AwsCognitoSettings__Authority": "https://cognito-idp.eu-west-1.amazonaws.com/eu-west-1_WWflkHNwn",
                                          "AwsCognitoSettings__Audience": "7oup5j6busol448gnor8lv0qi6",
                                          "AwsStepFunctionSetting__DeployAwsStateMachineArn": "arn:aws:states:eu-west-1:783764573141:stateMachine:sdc-capability-rollout",
                                          "OpenApiInfos__Versions__0__Audience": "company-internal",
                                          "OpenApiInfos__Versions__0__ContactEmail": "philip.pregler@siemens.com",
                                          "OpenApiInfos__Versions__0__ContactName": "Pulse Core API Team",
                                          "OpenApiInfos__Versions__0__ContactUrl": "https://www.pulse.de",
                                          "OpenApiInfos__Versions__0__Description": "API which provides all core functions",
                                          "OpenApiInfos__Versions__0__Id": "5064d915-f010-4223-956f-8f18e3ac43d0",
                                          "OpenApiInfos__Versions__0__Title": "API V1 - In Test"
                                        }
                                        """;

        public async Task GenerateAsync(FileInfo projectFileInfo,
                                        XDocument projectDocument,
                                        DotNetToolInfos dotNetToolInfos,
                                        ProjectFile? webApiProject)
        {
            // 1. CliRunner
            var filePath = Path.Combine(projectFileInfo.Directory!.FullName, "Properties", "EnvironmentVariables.json");
            var fileInfo = new FileInfo(filePath);

            var newTemplate = Template.Replace("$namespace$", $"{dotNetToolInfos.ProjectName}.Test")
                                      .Replace("$dotNetToolName$", dotNetToolInfos.NormalizedName);

            var formattedTemplate = newTemplate.FormatSyntaxTree();

            if (fileInfo.Directory!.NotExists())
            {
                fileInfo.Directory!.Create();
            }

            await File.WriteAllTextAsync(fileInfo.FullName, formattedTemplate).ConfigureAwait(false);

            // 2. Print success message
            consoleService.WriteSuccess($"Successfully created {fileInfo.FullName}");
        }
    }
}
