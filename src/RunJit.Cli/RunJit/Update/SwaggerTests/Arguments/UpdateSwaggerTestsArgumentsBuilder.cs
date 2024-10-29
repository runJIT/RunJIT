//using Extensions.Pack;
//using Microsoft.Extensions.DependencyInjection;

//namespace RunJit.Cli.RunJit.Update.SwaggerTests
//{
//    internal static class AddUpdateSwaggerTestsArgumentsBuilderExtension
//    {
//        internal static void AddUpdateSwaggerTestsArgumentsBuilder(this IServiceCollection services)
//        {
//            services.AddSingletonIfNotExists<IUpdateSwaggerTestsArgumentsBuilder, UpdateSwaggerTestsArgumentsBuilder>();
//        }
//    }

//    internal interface IUpdateSwaggerTestsArgumentsBuilder
//    {
//        IEnumerable<System.CommandLine.Argument> Build();
//    }

//    internal sealed class UpdateSwaggerTestsArgumentsBuilder : IUpdateSwaggerTestsArgumentsBuilder
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


