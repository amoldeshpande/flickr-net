using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using FlickrNet;
using System.Reactive.Subjects;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Shouldly;

namespace FlickrNetTest.Async
{
    [Trait("Category","AccessTokenRequired")]
    public class StatsAsyncTests : BaseTest
    {
        [Fact]
        public async Task StatsGetCollectionDomainsAsyncTest()
        {
            Flickr f = AuthInstance;

            DateTime d = DateTime.Today.AddDays(-7);

            var w = new AsyncSubject<FlickrResult<StatDomainCollection>>();
            var r = await f.StatsGetCollectionDomainsAsync(d, 1, 25);
            w.OnNext(r); w.OnCompleted();

            var result = w.Next().First();
            Assert.False(result.HasError);
        }

        [Fact]
        public async Task StatsGetPhotoDomainsAsyncTest()
        {
            Flickr f = AuthInstance;

            DateTime d = DateTime.Today.AddDays(-7);

            var w = new AsyncSubject<FlickrResult<StatDomainCollection>>();
            var r = await f.StatsGetPhotoDomainsAsync(d, 1, 25);
             { w.OnNext(r); w.OnCompleted(); }

            var result = w.Next().First();
            Assert.False(result.HasError);
        }

        [Fact]
        public async Task StatsGetPhotostreamDomainsAsyncTest()
        {
            Flickr f = AuthInstance;

            DateTime d = DateTime.Today.AddDays(-7);

            var w = new AsyncSubject<FlickrResult<StatDomainCollection>>();
            var r = await f.StatsGetPhotostreamDomainsAsync(d, 1, 25);
            { w.OnNext(r); w.OnCompleted(); };

            var result = w.Next().First();
            Assert.False(result.HasError);
        }

        [Fact]
        public async Task StatsGetPhotosetDomainsAsyncTest()
        {
            Flickr f = AuthInstance;

            DateTime d = DateTime.Today.AddDays(-7);

            var w = new AsyncSubject<FlickrResult<StatDomainCollection>>();
            var r = await f.StatsGetPhotosetDomainsAsync(d, 1, 25);
             { w.OnNext(r); w.OnCompleted(); };

            var result = w.Next().First();
            Assert.False(result.HasError);
        }


        [Fact]
        public async Task StatsGetCollectionStatsAsyncTest()
        {
            Flickr f = AuthInstance;

            var collection = f.CollectionsGetTree().First();

            DateTime d = DateTime.Today.AddDays(-7);

            var w = new AsyncSubject<FlickrResult<Stats>>();
            var r = await f.StatsGetCollectionStatsAsync(d, collection.CollectionId);
             { w.OnNext(r); w.OnCompleted(); };

            var result = w.Next().First();
            Assert.False(result.HasError);

        }

        [Fact]
        public async Task StatsGetPhotoStatsAsyncTest()
        {
            Flickr.CacheDisabled = true;

            Flickr f = AuthInstance;

            DateTime d = DateTime.Today.AddDays(-7);

            var w = new AsyncSubject<FlickrResult<Stats>>();
            var r = await f.StatsGetPhotoStatsAsync(d, "7176125763");
             { w.OnNext(r); w.OnCompleted(); };

            var result = w.Next().First();
            if (result.HasError) throw result.Error;

            Assert.False(result.HasError);
        }

        [Fact]
        public async Task StatsGetPhotostreamStatsAsyncTest()
        {
            Flickr f = AuthInstance;

            var range = Enumerable.Range(7, 5);
            var list = new List<Stats>();

            foreach(var i in range)
            {
                var d = DateTime.Today.AddDays(-i);

                var w = new AsyncSubject<FlickrResult<Stats>>();
                var r = await f.StatsGetPhotostreamStatsAsync(d);
                 { w.OnNext(r); w.OnCompleted(); };

                var result = w.Next().First();

                result.HasError.ShouldBe(false);
                result.Result.ShouldNotBe(null);

                list.Add(result.Result);
            }

            list.Count.ShouldBe(5);
            list.ShouldContain(s => s.Views > 0);
        }
    }
}
