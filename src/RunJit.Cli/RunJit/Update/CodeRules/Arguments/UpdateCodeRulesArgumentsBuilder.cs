//using Extensions.Pack;
//using Microsoft.Extensions.DependencyInjection;

//namespace RunJit.Cli.RunJit.Update.CodeRules
//{
//    internal static class AddUpdateCodeRulesArgumentsBuilderExtension
//    {
//        internal static void AddUpdateCodeRulesArgumentsBuilder(this IServiceCollection services)
//        {
//            services.AddSingletonIfNotExists<IUpdateCodeRulesArgumentsBuilder, UpdateCodeRulesArgumentsBuilder>();
//        }
//    }

//    internal interface IUpdateCodeRulesArgumentsBuilder
//    {
//        IEnumerable<System.CommandLine.Argument> Build();
//    }

//    internal sealed class UpdateCodeRulesArgumentsBuilder : IUpdateCodeRulesArgumentsBuilder
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


