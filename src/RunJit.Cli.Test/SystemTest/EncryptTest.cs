using System.Diagnostics;
using AspNetCore.Simple.Sdk.Mediator;
using Extensions.Pack;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RunJit.Cli.Test.SystemTest
{
    [TestCategory("runjit encrypt")]
    [TestClass]
    public class EncryptTest : GlobalSetup
    {
        [TestMethod]
        public async Task Encrypt_String_Value()
        {
            // 1. Create new Web Api
            var stringToEncrypt = "Hello World";

            // 2. Create Web-Api endpoints
            var encryptedString = await Mediator.Send(new EncryptString(stringToEncrypt));

            // 3. Test that not same string is containing
            Assert.AreNotEqual(encryptedString, stringToEncrypt);

            // 4. Decrypt string
            var decryptedString = await Mediator.Send(new DecryptString(encryptedString));

            // 5. Check that after decryption we have the same value
            Assert.AreEqual(stringToEncrypt, decryptedString);
        }
    }

    internal sealed record EncryptString(string Value) : ICommand<string>;

    internal sealed class EncryptStringHandler : ICommandHandler<EncryptString, string>
    {
        public async Task<string> Handle(EncryptString request, CancellationToken cancellationToken)
        {
            await using var sw = new StringWriter();
            Console.SetOut(sw);

            var strings = CollectConsoleParameters(request).ToArray();
            var consoleCall = strings.Flatten(" ");
            Console.WriteLine();
            Console.WriteLine(consoleCall);
            Debug.WriteLine(consoleCall);
            var exitCode = await Program.Main(strings);
            var output = sw.ToString();

            Assert.AreEqual(0, exitCode, output);

            var encryptedString = output.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Last();
            return encryptedString;
        }

        private IEnumerable<string> CollectConsoleParameters(EncryptString paramters)
        {
            // 1. Parameter solution file from the backend to parse
            yield return "runjit";
            yield return "encrypt";
            yield return paramters.Value;
        }
    }
}
