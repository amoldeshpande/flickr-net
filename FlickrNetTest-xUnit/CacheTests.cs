using System;

using Xunit;
using FlickrNet;
using System.IO;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for CacheTests
    /// </summary>
    
    public class CacheTests : BaseTest
    {
        [Fact]
        public void CacheLocationTest()
        {
            string origLocation = Flickr.CacheLocation;

            Console.WriteLine(origLocation);

            string newLocation = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            Flickr.CacheLocation = newLocation;

            Assert.Equal(Flickr.CacheLocation, newLocation);

            Flickr.CacheLocation = origLocation;

            Assert.Equal(Flickr.CacheLocation, origLocation);

        }

        [Fact]
        public void CacheHitTest()
        {
            if (Directory.Exists(Flickr.CacheLocation))
            {
                Directory.Delete(Flickr.CacheLocation, true);
            }

            Flickr f = Instance;
            Flickr.FlushCache();
            f.InstanceCacheDisabled = false;

            f.PeopleGetPublicPhotos(TestData.TestUserId);

            string lastUrl = f.LastRequest;

            ICacheItem item = Cache.Responses.Get(lastUrl, TimeSpan.MaxValue, false);

            Assert.NotNull(item);
            Assert.IsType<ResponseCacheItem>(item);

            var response = item as ResponseCacheItem;

            Assert.NotNull(response.Url);
            Assert.Equal(lastUrl, response.Url.AbsoluteUri);//, "Url should match the url requested from the cache."
        }
    }
}
