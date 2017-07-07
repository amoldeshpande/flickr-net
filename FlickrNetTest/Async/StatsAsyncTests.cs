using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using FlickrNet;
using Shouldly;
using System.Threading.Tasks;

namespace FlickrNetTest.Async
{
    [TestFixture]
    [Category("AccessTokenRequired")]
    public class StatsAsyncTests : BaseTest
    {
        [Test]
        public async Task StatsGetCollectionDomainsAsyncTest()
        {
            Flickr f = AuthInstance;

            DateTime d = DateTime.Today.AddDays(-7);

            var result = await f.StatsGetCollectionDomainsAsync(d, 1, 25);

            Assert.IsFalse(result.HasError);
        }

        [Test]
        public async Task StatsGetPhotoDomainsAsyncTest()
        {
            Flickr f = AuthInstance;

            DateTime d = DateTime.Today.AddDays(-7);

            var result = await f.StatsGetPhotoDomainsAsync(d, 1, 25);
            Assert.IsFalse(result.HasError);
        }

        [Test]
        public async Task StatsGetPhotostreamDomainsAsyncTest()
        {
            Flickr f = AuthInstance;

            DateTime d = DateTime.Today.AddDays(-7);

            var result = await f.StatsGetPhotostreamDomainsAsync(d, 1, 25);
            Assert.IsFalse(result.HasError);
        }

        [Test]
        public async Task StatsGetPhotosetDomainsAsyncTest()
        {
            Flickr f = AuthInstance;

            DateTime d = DateTime.Today.AddDays(-7);

            var result = await f.StatsGetPhotosetDomainsAsync(d, 1, 25);
            Assert.IsFalse(result.HasError);
        }


        [Test]
        public async Task StatsGetCollectionStatsAsyncTest()
        {
            Flickr f = AuthInstance;

            var collection = f.CollectionsGetTree().First();

            DateTime d = DateTime.Today.AddDays(-7);

            var result = await f.StatsGetCollectionStatsAsync(d, collection.CollectionId);

            Assert.IsFalse(result.HasError);

        }

        [Test]
        public async Task StatsGetPhotoStatsAsyncTest()
        {
            Flickr.CacheDisabled = true;

            Flickr f = AuthInstance;

            DateTime d = DateTime.Today.AddDays(-7);

            var result = await f.StatsGetPhotoStatsAsync(d, "7176125763");
            if (result.HasError) throw result.Error;

            Assert.IsFalse(result.HasError);
        }

        [Test]
        public async Task StatsGetPhotostreamStatsAsyncTest()
        {
            Flickr f = AuthInstance;

            var range = Enumerable.Range(7, 5);
            var list = new List<Stats>();

            foreach(var i in range)
            {
                var d = DateTime.Today.AddDays(-i);

                var result = await f.StatsGetPhotostreamStatsAsync(d);

                result.HasError.ShouldBe(false);
                result.Result.ShouldNotBe(null);

                list.Add(result.Result);
            }

            list.Count.ShouldBe(5);
            list.ShouldContain(s => s.Views > 0);
        }
    }
}
