using System.CommandLine;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.RunJit.New.Lambda
{
    public static class AddLambdaOptionsBuilderExtension
    {
        public static void AddLambdaOptionsBuilder(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<ILambdaOptionsBuilder, LambdaOptionsBuilder>();
        }
    }

    internal interface ILambdaOptionsBuilder
    {
        IEnumerable<Option> Build();
    }

    internal class LambdaOptionsBuilder : ILambdaOptionsBuilder
    {
        public IEnumerable<Option> Build()
        {
            yield return BuildSolutionOption();
            yield return BuildModuleNameOption();
            yield return BuildFunctionNameOption();
            yield return BuildLambdaNameOption();
            yield return BasedOnBranch();
            yield return WorkingDirectory();
        }

        private Option BuildSolutionOption()
        {
            return new Option(new[] { "--solution", "-ts" }, "Target solution")
            {
                Required = true,
                Argument = new Argument<FileInfo>("solution")
                {
                    Description = "The Filepath to your solution, where the lambda should be integrated"
                }
            };
        }
        private Option BuildModuleNameOption()
        {
            return new Option(new[] { "--module-name", "-mn" }, "The name of your backend module (i.e. \"Core\", \"Survey\", ...)")
            {
                Required = true,
                Argument = new Argument<string>("moduleName")
                {
                    Description = "The name of your backend module (i.e. \"Core\", \"Survey\", ...)"
                }
            };
        }
        private Option BuildFunctionNameOption()
        {
            return new Option(new[] { "--function-name", "-fn" }, "The name of the function, that will be invoked")
            {
                Required = true,
                Argument = new Argument<string>("functionName")
                {
                    Description = "The name of the function, that will be invoked"
                }
            };
        }
        private Option BuildLambdaNameOption()
        {
            return new Option(new[] { "--lambda-name", "-ln" }, "The deployment name of the lambda")
            {
                Required = true,
                Argument = new Argument<string>("lambdaName")
                {
                    Description = "The deployment name of the lambda"
                }
            };
        }
        
        public Option BasedOnBranch()
        {
            return new Option(new[] { "--branch", "-b" }, "Give the reference branch for. If you update code rules by using git url to clone please provide the reference branch from")
            {
                Required = false,
                Argument = new Argument<string>("branch")
                {
                    Description = "Give the reference branch for. If you update code rules by using git url to clone please provide the reference branch from"
                }
            };
        }
        
        public Option WorkingDirectory()
        {
            return new Option(new[] { "--working-directory", "-wd" }, "The working directory in which all operation should be executed")
            {
                Required = false,
                Argument = new Argument<string>("workingDirectory")
                {
                    Description = "The working directory in which all operation should be executed"
                }
            };
        }
    }
}
