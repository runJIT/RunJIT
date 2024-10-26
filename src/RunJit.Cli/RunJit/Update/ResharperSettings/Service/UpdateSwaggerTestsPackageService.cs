//using DotNetTool.Service;
//using Extensions.Pack;
//using Microsoft.Extensions.DependencyInjection;

//namespace RunJit.Cli.RunJit.Update.SwaggerTests
//{
//    public static class AddUpdateResharperSettingsPackageServiceExtension
//    {
//        public static void AddUpdateResharperSettingsPackageService(this IServiceCollection services)
//        {
//            services.AddSingletonIfNotExists<IUpdateResharperSettingsPackageService, UpdateResharperSettingsPackageService>();
//        }
//    }

//    internal interface IUpdateResharperSettingsPackageService
//    {
//        Task UpdateResharperSettingsPackageAsync(OutdatedSwaggerTestsResponse outdatedSwaggerTestsResponse);
//    }

//    internal class UpdateResharperSettingsPackageService(ConsoleService consoleService,
//                                                 IDotNetTool dotNetTool) : IUpdateResharperSettingsPackageService
//    {
//        public async Task UpdateResharperSettingsPackageAsync(FileInfo solutionFileInfo)
//        {

//        }
//    }
//}


