using System.Diagnostics;
using AspNetCore.Simple.Sdk.Mediator;
using Extensions.Pack;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RunJit.Cli.Test.SystemTest
{
    [TestCategory("runjit decrypt")]
    [TestClass]
    public class DecryptTest : GlobalSetup
    {
        [TestMethod]
        public async Task Decrypt_String_Value()
        {
            // 1. Create new Web Api
            var stringToDecrypt = "Hello World";

            // 2. Create Web-Api endpoints
            var encryptedString = await Mediator.Send(new EncryptString(stringToDecrypt)).ConfigureAwait(false);

            // 3. Test that not same string is containing
            Assert.AreNotEqual(encryptedString, stringToDecrypt);

            // 4. Decrypt string
            var decryptedString = await Mediator.Send(new DecryptString(encryptedString)).ConfigureAwait(false);

            // 5. Check that after decryption we have the same value
            Assert.AreEqual(stringToDecrypt, decryptedString);
        }
    }

    internal sealed record DecryptString(string Value) : ICommand<string>;

    internal sealed class DecryptStringHandler : ICommandHandler<DecryptString, string>
    {
        public async Task<string> Handle(DecryptString request,
                                         CancellationToken cancellationToken)
        {
            await using var sw = new StringWriter();
            Console.SetOut(sw);

            var strings = CollectConsoleParameters(request).ToArray();
            var consoleCall = strings.Flatten(" ");
            Console.WriteLine();
            Console.WriteLine(consoleCall);
            Debug.WriteLine(consoleCall);
            var exitCode = await Program.Main(strings).ConfigureAwait(false);
            var output = sw.ToString();

            Assert.AreEqual(0, exitCode, output);

            var decryptedString = output.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Last();

            return decryptedString;
        }

        private IEnumerable<string> CollectConsoleParameters(DecryptString paramters)
        {
            // 1. Parameter solution file from the backend to parse
            yield return "runjit";
            yield return "decrypt";
            yield return paramters.Value;
        }
    }
}
