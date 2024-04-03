//using DotNetTool.Service;
//using Extensions.Pack;
//using Microsoft.Extensions.DependencyInjection;

//namespace RunJit.Cli.RunJit.Update.SwaggerTests
//{
//    public static class AddUpdateSwaggerTestsPackageServiceExtension
//    {
//        public static void AddUpdateSwaggerTestsPackageService(this IServiceCollection services)
//        {
//            services.AddSingletonIfNotExists<IUpdateSwaggerTestsPackageService, UpdateSwaggerTestsPackageService>();
//        }
//    }
    
//    internal interface IUpdateSwaggerTestsPackageService
//    {
//        Task UpdateSwaggerTestsPackageAsync(OutdatedSwaggerTestsResponse outdatedSwaggerTestsResponse);
//    }

//    internal class UpdateSwaggerTestsPackageService(IConsoleService consoleService,
//                                                 IDotNetTool dotNetTool) : IUpdateSwaggerTestsPackageService
//    {
//        public async Task UpdateSwaggerTestsPackageAsync(FileInfo solutionFileInfo)
//        {
            
//        }
//    }
//}
