using Argument.Check;
using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.Services
{
    internal static class AddCollectTillInputCorrectExtension
    {
        internal static void AddCollectTillInputCorrect(this IServiceCollection services)
        {
            services.AddConsoleService();

            services.AddSingletonIfNotExists<ICollectTillInputCorrect, CollectTillInputCorrect>();
        }
    }

    public interface ICollectTillInputCorrect
    {
        string CollectTillInputIsValid(string messageForUser,
                                       IInputValidator inputValidator);

        string CollectTillInputIsValid(string messageForUser,
                                       Predicate<string> inputValidation,
                                       Func<string, string> getErrorMessageForInput);
    }

    internal class CollectTillInputCorrect(ConsoleService consoleService) : ICollectTillInputCorrect
    {
        public string CollectTillInputIsValid(string messageForUser,
                                              Predicate<string> inputValidation,
                                              Func<string, string> getErrorMessageForInput)
        {
            Throw.IfNullOrWhiteSpace(messageForUser);
            Throw.IfNull(inputValidation);

            var isValid = false;
            var input = string.Empty;

            while (isValid.IsFalse())
            {
                consoleService.WriteInput(messageForUser);
                input = consoleService.ReadLine().Trim();
                isValid = inputValidation(input);

                if (isValid.IsFalse())
                {
                    consoleService.WriteError(getErrorMessageForInput(input));
                    consoleService.WriteLine();
                }
            }

            return input;
        }

        public string CollectTillInputIsValid(string messageForUser,
                                              IInputValidator inputValidator)
        {
            Throw.IfNullOrWhiteSpace(messageForUser);
            Throw.IfNull(inputValidator);

            var input = string.Empty;
            ValidationResult? validationResult = null;

            while (validationResult.IsNull() || validationResult.IsValid.IsFalse())
            {
                consoleService.WriteInput(messageForUser);
                input = consoleService.ReadLine().Trim();
                validationResult = inputValidator.Validate(input);

                if (validationResult.IsValid.IsFalse())
                {
                    consoleService.WriteError(validationResult.Errors);
                    consoleService.WriteLine();
                }
            }

            return input;
        }
    }
}
