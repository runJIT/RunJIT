//using Extensions.Pack;
//using Microsoft.Extensions.DependencyInjection;

//namespace RunJit.Cli.RunJit.Update.Backend.CodeRules
//{
//    public static class AddUpdateCodeRulesArgumentsBuilderExtension
//    {
//        public static void AddUpdateCodeRulesArgumentsBuilder(this IServiceCollection services)
//        {
//            services.AddSingletonIfNotExists<IUpdateCodeRulesArgumentsBuilder, UpdateCodeRulesArgumentsBuilder>();
//        }
//    }

//    internal interface IUpdateCodeRulesArgumentsBuilder
//    {
//        IEnumerable<System.CommandLine.Argument> Build();
//    }

//    internal class UpdateCodeRulesArgumentsBuilder : IUpdateCodeRulesArgumentsBuilder
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
