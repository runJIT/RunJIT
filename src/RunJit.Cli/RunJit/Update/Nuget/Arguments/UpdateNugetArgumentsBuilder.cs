//using Extensions.Pack;
//using Microsoft.Extensions.DependencyInjection;

//namespace RunJit.Cli.RunJit.Update.Nuget
//{
//    internal static class AddUpdateNugetArgumentsBuilderExtension
//    {
//        internal static void AddUpdateNugetArgumentsBuilder(this IServiceCollection services)
//        {
//            services.AddSingletonIfNotExists<IUpdateNugetArgumentsBuilder, UpdateNugetArgumentsBuilder>();
//        }
//    }

//    internal interface IUpdateNugetArgumentsBuilder
//    {
//        IEnumerable<System.CommandLine.Argument> Build();
//    }

//    internal sealed class UpdateNugetArgumentsBuilder : IUpdateNugetArgumentsBuilder
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


