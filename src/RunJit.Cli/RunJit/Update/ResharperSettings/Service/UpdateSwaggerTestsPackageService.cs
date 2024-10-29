//using DotNetTool.Service;
//using Extensions.Pack;
//using Microsoft.Extensions.DependencyInjection;

//namespace RunJit.Cli.RunJit.Update.SwaggerTests
//{
//    internal static class AddUpdateResharperSettingsPackageServiceExtension
//    {
//        internal static void AddUpdateResharperSettingsPackageService(this IServiceCollection services)
//        {
//            services.AddSingletonIfNotExists<IUpdateResharperSettingsPackageService, UpdateResharperSettingsPackageService>();
//        }
//    }

//    internal interface IUpdateResharperSettingsPackageService
//    {
//        Task UpdateResharperSettingsPackageAsync(OutdatedSwaggerTestsResponse outdatedSwaggerTestsResponse);
//    }

//    internal sealed class UpdateResharperSettingsPackageService(ConsoleService consoleService,
//                                                 IDotNetTool dotNetTool) : IUpdateResharperSettingsPackageService
//    {
//        public async Task UpdateResharperSettingsPackageAsync(FileInfo solutionFileInfo)
//        {

//        }
//    }
//}


