//using DotNetTool.Service;
//using Extensions.Pack;
//using Microsoft.Extensions.DependencyInjection;

//namespace RunJit.Cli.RunJit.Update.SwaggerTests
//{
//    internal static class AddUpdateSwaggerTestsPackageServiceExtension
//    {
//        internal static void AddUpdateSwaggerTestsPackageService(this IServiceCollection services)
//        {
//            services.AddSingletonIfNotExists<IUpdateSwaggerTestsPackageService, UpdateSwaggerTestsPackageService>();
//        }
//    }

//    internal interface IUpdateSwaggerTestsPackageService
//    {
//        Task UpdateSwaggerTestsPackageAsync(OutdatedSwaggerTestsResponse outdatedSwaggerTestsResponse);
//    }

//    internal sealed class UpdateSwaggerTestsPackageService(ConsoleService consoleService,
//                                                 IDotNetTool dotNetTool) : IUpdateSwaggerTestsPackageService
//    {
//        public async Task UpdateSwaggerTestsPackageAsync(FileInfo solutionFileInfo)
//        {

//        }
//    }
//}


