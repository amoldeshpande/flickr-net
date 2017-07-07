using System.Collections.Generic;

using Xunit;
using FlickrNet;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for TestTests
    /// </summary>
    
    public class TestTests : BaseTest
    {
        [Fact]
        public void TestGenericGroupSearch()
        {
            Flickr f = Instance;

            var parameters = new Dictionary<string, string>();
            parameters.Add("text", "Flowers");
            UnknownResponse response = f.TestGeneric("flickr.groups.search", parameters);

            Assert.NotNull(response);// "UnknownResponse should not be null.");
            Assert.NotNull(response.ResponseXml);// "ResponseXml should not be null.");

        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public void TestGenericTestNull()
        {
            Flickr f = AuthInstance;

            UnknownResponse response = f.TestGeneric("flickr.test.null", null);

            Assert.NotNull(response);// "UnknownResponse should not be null.");
            Assert.NotNull(response.ResponseXml);// "ResponseXml should not be null.");
        }

        [Fact]
        public void TestEcho()
        {
            Flickr f = Instance;
            var parameters = new Dictionary<string, string>();
            parameters.Add("test1", "testvalue");

            Dictionary<string, string> returns = f.TestEcho(parameters);

            Assert.NotNull(returns);

            // Was 3, now 11 because of extra oauth parameter used by default.
            Assert.Equal(11, returns.Count);

            Assert.Equal("flickr.test.echo", returns["method"]);
            Assert.Equal("testvalue", returns["test1"]);

        }
    }
}
