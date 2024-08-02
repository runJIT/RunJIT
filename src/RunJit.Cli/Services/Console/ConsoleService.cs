using Extensions.Pack;
using Microsoft.Extensions.DependencyInjection;

namespace RunJit.Cli
{
    internal static class AddConsoleServiceExtension
    {
        internal static void AddConsoleService(this IServiceCollection services)
        {
            services.AddSingletonIfNotExists<IConsoleService, ConsoleService>();
        }
    }

    public interface IConsoleService
    {
        string ReadLine();

        void WriteInput(string value);

        void WriteSample(string value);

        void WriteError(string value);

        void WriteSuccess(string value);

        void WriteInfo(string value);

        void WriteLine();
    }

    internal class ConsoleService : IConsoleService
    {
        public void WriteLine()
        {
            Console.WriteLine();
        }

        public void WriteInfo(string value)
        {
            WriteLine(value);
        }

        public void WriteInput(string value)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            WriteLine(value);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void WriteSuccess(string value)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            WriteLine(value);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public string ReadLine()
        {
            var result = Console.ReadLine();
            Console.WriteLine();

            return result ?? string.Empty;
        }

        public void WriteSample(string value)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(value);
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void WriteError(string value)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            WriteLine(value);
            Console.ForegroundColor = ConsoleColor.White;
        }

        private void WriteLine(string value)
        {
            Console.WriteLine(value);
        }
    }
}
