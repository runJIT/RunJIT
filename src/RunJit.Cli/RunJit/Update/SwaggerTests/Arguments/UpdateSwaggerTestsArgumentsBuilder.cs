//using Extensions.Pack;
//using Microsoft.Extensions.DependencyInjection;

//namespace RunJit.Cli.RunJit.Update.SwaggerTests
//{
//    public static class AddUpdateSwaggerTestsArgumentsBuilderExtension
//    {
//        public static void AddUpdateSwaggerTestsArgumentsBuilder(this IServiceCollection services)
//        {
//            services.AddSingletonIfNotExists<IUpdateSwaggerTestsArgumentsBuilder, UpdateSwaggerTestsArgumentsBuilder>();
//        }
//    }

//    internal interface IUpdateSwaggerTestsArgumentsBuilder
//    {
//        IEnumerable<System.CommandLine.Argument> Build();
//    }

//    internal class UpdateSwaggerTestsArgumentsBuilder : IUpdateSwaggerTestsArgumentsBuilder
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


