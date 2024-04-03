//using Extensions.Pack;
//using Microsoft.Extensions.DependencyInjection;

//namespace RunJit.Cli.RunJit.Update.SwaggerTests
//{
//    public static class AddUpdateResharperSettingsArgumentsBuilderExtension
//    {
//        public static void AddUpdateResharperSettingsArgumentsBuilder(this IServiceCollection services)
//        {
//            services.AddSingletonIfNotExists<IUpdateResharperSettingsArgumentsBuilder, UpdateResharperSettingsArgumentsBuilder>();
//        }
//    }

//    internal interface IUpdateResharperSettingsArgumentsBuilder
//    {
//        IEnumerable<System.CommandLine.Argument> Build();
//    }

//    internal class UpdateResharperSettingsArgumentsBuilder : IUpdateResharperSettingsArgumentsBuilder
//    {
//        public IEnumerable<System.CommandLine.Argument> Build()
//        {
//            yield return BuildSourceOption();
//        }

//        public System.CommandLine.Argument BuildSourceOption()
//        {
//            return new System.CommandLine.Argument<string>()
//            {
//                Name = "solutionFile",
//            };
//        }
//    }
//}
