//using DotNetTool.Service;
//using Extensions.Pack;
//using Microsoft.Extensions.DependencyInjection;

//namespace RunJit.Cli.RunJit.Update.CodeRules
//{
//    public static class AddUpdateCodeRulesPackageServiceExtension
//    {
//        public static void AddUpdateCodeRulesPackageService(this IServiceCollection services)
//        {
//            services.AddSingletonIfNotExists<IUpdateCodeRulesPackageService, UpdateCodeRulesPackageService>();
//        }
//    }

//    internal interface IUpdateCodeRulesPackageService
//    {
//        Task UpdateCodeRulesPackageAsync(OutdatedCodeRulesResponse outdatedCodeRulesResponse);
//    }

//    internal sealed class UpdateCodeRulesPackageService(ConsoleService consoleService,
//                                                 IDotNetTool dotNetTool) : IUpdateCodeRulesPackageService
//    {
//        public async Task UpdateCodeRulesPackageAsync(FileInfo solutionFileInfo)
//        {

//        }
//    }
//}


