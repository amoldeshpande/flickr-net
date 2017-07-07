using FlickrNet;
using Xunit;
using System;
using System.Xml;

#if !DOTNETSTANDARD
namespace FlickrNetTest
{
    
    
    /// <summary>
    ///This is a test class for FlickrConfigurationSettingsTest and is intended
    ///to contain all FlickrConfigurationSettingsTest Unit Tests
    ///</summary>
    
    public class FlickrConfigurationSettingsTest : BaseTest
    {

        /// <summary>
        ///A test for FlickrConfigurationSettings Constructor
        ///</summary>
        [Fact]
        public void FlickrConfigurationSettingsConstructorTest()
        {
            const string xml = "<flickrNet apiKey=\"apikey\" secret=\"secret\" token=\"thetoken\" " +
                               "cacheDisabled=\"true\" cacheSize=\"1024\" cacheTimeout=\"01:00:00\" " +
                               "cacheLocation=\"testlocation\" service=\"flickr\">"
                               + "<proxy ipaddress=\"localhost\" port=\"8800\" username=\"testusername\" " +
                               "password=\"testpassword\" domain=\"testdomain\"/>"
                               + "</flickrNet>";
            var doc = new XmlDocument();
            doc.LoadXml(xml);

            var configNode = doc.SelectSingleNode("flickrNet");
            var target = new FlickrConfigurationSettings(configNode);

            Assert.Equal("apikey", target.ApiKey);
            Assert.Equal("secret", target.SharedSecret);
            Assert.Equal("thetoken", target.ApiToken);
            Assert.True(target.CacheDisabled);
            Assert.Equal(1024, target.CacheSize);
            Assert.Equal(new TimeSpan(1, 0, 0), target.CacheTimeout);
            Assert.Equal("testlocation", target.CacheLocation);

            Assert.True(target.IsProxyDefined, "IsProxyDefined should be true");
            Assert.Equal("localhost", target.ProxyIPAddress);
            Assert.Equal(8800, target.ProxyPort);
            Assert.Equal("testusername", target.ProxyUsername);
            Assert.Equal("testpassword", target.ProxyPassword);
            Assert.Equal("testdomain", target.ProxyDomain);
        }
    }
}
#endif // !DOTNETSTANDARD
