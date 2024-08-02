using Extensions.Pack;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RunJit.Cli.Test
{
    [TestClass]
    public class HttpHeaderToCopyTest
    {
        [TestMethod]
        public void How_To_Copy_All_Headers()
        {
            var context1 = new DefaultHttpContext();
            context1.Request.Headers.Append("a", "a");
            context1.Request.Headers.Append("b", "b");
            context1.Request.Headers.Append("c", "c");
            context1.Request.Headers.Append("d", "d");

            var context2 = new DefaultHttpContext();
            context2.Request.Headers.AddRange(context1.Request.Headers);

            var dictionarySource = context1.Request.Headers.ToDictionary();
            var dictionaryTarget = context2.Request.Headers.ToDictionary();

            Assert.AreEqual(dictionarySource.ToJson(), dictionaryTarget.ToJson());
        }

        [TestMethod]
        public void How_To_Copy_All_Headers_To_HttpClient()
        {
            var context1 = new DefaultHttpContext();
            context1.Request.Headers.Append("a", "a");
            context1.Request.Headers.Append("b", "b");
            context1.Request.Headers.Append("c", "c");
            context1.Request.Headers.Append("d", "d");

            var httpClient = new HttpClient();
            context1.Request.Headers.ForEach(headerEntry => httpClient.DefaultRequestHeaders.Add(headerEntry.Key, headerEntry.Value.Select(item => item)));

            var dictionarySource = context1.Request.Headers.ToDictionary();
            var dictionaryTarget = httpClient.DefaultRequestHeaders.ToDictionary();

            Assert.AreEqual(dictionarySource.ToJson(), dictionaryTarget.ToJson());
        }
    }
}
