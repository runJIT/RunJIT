using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli.Services
{
    public static class AddRetryHelperExtension
    {
        public static void AddRetryHelper(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<RetryHelper>();
        }
    }

    internal class RetryHelper
    {
        internal async Task ExecuteWithRetryAsync(Action action,
                                                  int maxRetries = 3,
                                                  int delayPerRetry = 100)
        {
            int retryCount = 0;
            while (true)
            {
                try
                {
                    action();
                    break; // Exit if successful
                }
                catch (Exception ex)
                {
                    retryCount++;
                    if (retryCount >= maxRetries)
                    {
                        Console.WriteLine("Max retries reached. Operation failed.");
                        throw; // Rethrow the exception after max retries
                    }
                    Console.WriteLine($"Retry {retryCount}/{maxRetries} after exception: {ex.Message}");
                    await Task.Delay(delayPerRetry); // Wait before retrying
                }
            }
        }
    }
}
