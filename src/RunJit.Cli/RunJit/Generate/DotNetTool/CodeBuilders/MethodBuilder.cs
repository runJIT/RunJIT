using System.Collections.Immutable;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;
using RunJit.Cli.Services;
using RunJit.Cli.Services.Endpoints;

namespace RunJit.Cli.RunJit.Generate.DotNetTool
{
    internal static class AddMethodBuilderExtension
    {
        internal static void AddMethodBuilder(this IServiceCollection services)
        {
            services.AddBuiltInTypeTableService();

            services.AddSingletonIfNotExists<MethodBuilder>();
        }
    }

    // What we are create here:
    // - We create for each http method / controller method the method which the clients need to call this endpoint
    // - Including all url parameters, payloads and relative url.
    // Sample:
    //
    // public Task<IEnumerable<AdminPrivilege>> GetAdminPrivilegeListAsync(int projectId, bool useCache = true)
    // {
    //     return _httpCallHandler.CallAsync<IEnumerable<AdminPrivilege>>(HttpMethod.Get, $"admin/project/{projectId}/privilege/list?useCache={useCache}", null);
    // }
    public class MethodBuilder(IBuiltInTypeTableService builtInTypeTableService)
    {
        private readonly string[] _httpActionWithPayloads = { "Post", "Patch", "Put" };

        private readonly string _methodTemplate = EmbeddedFile.GetFileContentFrom("Pulse.Generate.DotNetTool.Templates.method.rps");

        public IImmutableList<string> BuildFor(ControllerInfo controllerInfo)
        {
            return controllerInfo.Methods.Select(m => BuildFor(m, controllerInfo)).ToImmutableList();
        }

        public string BuildFor(MethodInfos methodInfo,
                               ControllerInfo controllerInfo)
        {
            // Any call to a http instance is never sync like like without a Task - we never do blocking API calls !!
            var normalizedReturnType = methodInfo.ResponseType.Original == "Task<ActionResult>" ||
                                       methodInfo.ResponseType.Original == "Task<IActionResult>" ||
                                       methodInfo.ResponseType.Original == "ActionResult" ||
                                       methodInfo.ResponseType.Original == "IActionResult" ||
                                       methodInfo.ResponseType.Original == "void"
                                           ? "Task"
                                           : methodInfo.ResponseType.Original;

            normalizedReturnType = normalizedReturnType.StartWith("Task").IsFalse() ? $"Task<{normalizedReturnType}>" : normalizedReturnType;

            var httpCallReturnType = methodInfo.ResponseType.Normalized.Contains("ActionResult") ||
                                     methodInfo.ResponseType.Normalized.ToLowerInvariant() == "void" ? string.Empty :
                                     normalizedReturnType == "Task" ? string.Empty : normalizedReturnType.Replace("Task<", "<");

            var payload = _httpActionWithPayloads.Contains(methodInfo.HttpAction) ? ", payload" : string.Empty;
            var httpDotNetToolCall = _httpActionWithPayloads.Contains(methodInfo.HttpAction) ? $"{methodInfo.HttpAction}AsJson" : methodInfo.HttpAction;

            // ToDo: detect which parameter is the payload !!
            var fromBody = methodInfo.Parameters.FirstOrDefault(p => p.Attributes.Any(a => a.Name == "FromBody"));

            var parameterBody = methodInfo.Parameters.FirstOrDefault(p => p.Attributes.IsEmpty() &&
                                                                          builtInTypeTableService.GetTypeFor(p.Type).IsNull());

            var payloadParameter = fromBody.IsNotNull() ? fromBody.Name : parameterBody?.Name;
            payloadParameter = payloadParameter.IsNullOrWhiteSpace() ? "null" : payloadParameter;

            // Any call to a http instance is never sync like like without a Task - we never do blocking API calls !!
            var methodName = methodInfo.SwaggerOperationId.IsNullOrWhiteSpace() ? methodInfo.Name : methodInfo.SwaggerOperationId.FirstCharToUpper();

            // Check for Async post fix ! very important in C# / Net environment -> No method without async at client level because we invoke http calls async !
            methodName = methodName.EndsWith("Async", StringComparison.Ordinal) ? methodName : $"{methodName}Async";

            var parameters = methodInfo.Parameters.Any()
                                 ? methodInfo.Parameters.Select(p =>
                                                                {
                                                                    var defaultValue = p.IsOptional ? $" = {p.DefaultValue}" : string.Empty;
                                                                    var nullable = p.DefaultValue == "null" ? p.Type.EndWith("?") ? string.Empty : "?" : string.Empty;
                                                                    var parameter = $"{p.Type}{nullable} {p.Name}{defaultValue}";
                                                                    parameter = parameter.Replace("IFormFile", nameof(InMemoryFileAsStream));

                                                                    return parameter;
                                                                }).Flatten(", ")
                                 : string.Empty;

            var controllerObsoleteAttribute = controllerInfo.Attributes.FirstOrDefault(a => a.Name == "Obsolete");
            var methodObsoleteAttribute = methodInfo.Attributes.FirstOrDefault(a => a.Name == "Obsolete");
            var attributes = methodObsoleteAttribute.IsNotNull() ? methodObsoleteAttribute.SyntaxTree : controllerObsoleteAttribute.IsNotNull() ? controllerObsoleteAttribute.SyntaxTree : string.Empty;
            var cancellationTokenParameter = methodInfo.Parameters.FirstOrDefault(p => p.Type == nameof(CancellationToken));
            var cancellationToken = cancellationTokenParameter.IsNotNull() ? cancellationTokenParameter.Name : $"{nameof(CancellationToken)}.{nameof(CancellationToken.None)}";

            var method = _methodTemplate.Replace("$returnType$", normalizedReturnType)
                                        .Replace("$name$", methodName)
                                        .Replace("$parameters$", parameters)
                                        .Replace("$url$", $@"""{methodInfo.RelativeUrl}""")
                                        .Replace("$httpMethod$", methodInfo.HttpAction)
                                        .Replace(", $payload$", payload)
                                        .Replace("$returnTypeNoTask$", httpCallReturnType)
                                        .Replace("$payloadAsJson$", payloadParameter)
                                        .Replace("$httpDotNetToolCall$", httpDotNetToolCall)
                                        .Replace("$attributes$", attributes)
                                        .Replace("$cancellationToken$", cancellationToken);

            return method;
        }
    }
}
