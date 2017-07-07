using System;
using System.Linq;
using Xunit;
using FlickrNet;
using System.Threading.Tasks;
using System.Reactive.Subjects;
using System.Reactive.Linq;
using Shouldly;

namespace FlickrNetTest.Async
{
    public class PhotosAsyncTests : BaseTest
    {
        [Fact]
        public async Task PhotosSearchRussianAsync()
        {
            var o = new PhotoSearchOptions();
            o.Extras = PhotoSearchExtras.Tags;
            o.Tags = "фото";
            o.PerPage = 100;

            Flickr f = Instance;

            var w = new AsyncSubject<FlickrResult<PhotoCollection>>();
            var r = await f.PhotosSearchAsync(o);
             { w.OnNext(r); w.OnCompleted(); };
            var result = w.Next().First();

            Assert.False(result.HasError);
            Assert.NotNull(result.Result);

            result.Result.Count.ShouldBeGreaterThan(0);

            var photos = result.Result;
            foreach (var photo in photos)
            {
                Console.WriteLine(photo.Title + " = " + string.Join(",", photo.Tags));
            }

        }
        [Fact]
        public async Task PhotosGetContactsPublicPhotosAsyncTest()
        {
            Flickr f = Instance;

            var w = new AsyncSubject<FlickrResult<PhotoCollection>>();
            var r = await f.PhotosGetContactsPublicPhotosAsync(TestData.TestUserId, 5, true, true, true, PhotoSearchExtras.All);
             { w.OnNext(r); w.OnCompleted(); };
            var result = w.Next().First();

            Assert.False(result.HasError);
            Assert.NotNull(result.Result);

            Assert.True(result.Result.Count > 0);// "Should return some photos.");
        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public async Task PhotosGetCountsAsyncTest()
        {
            DateTime date1 = DateTime.Today.AddMonths(-12);
            DateTime date2 = DateTime.Today.AddMonths(-6);
            DateTime date3 = DateTime.Today;

            DateTime[] uploadDates = { date1, date2, date3 };

            Flickr f = AuthInstance;

            var w = new AsyncSubject<FlickrResult<PhotoCountCollection>>();
            var r = await f.PhotosGetCountsAsync(uploadDates, false);
             { w.OnNext(r); w.OnCompleted(); };
            var result = w.Next().First();

            Assert.False(result.HasError);

            var counts = result.Result;

            Assert.Equal(2, counts.Count);// "Should be two counts returned."

            var count1 = counts[0];

            Assert.Equal(date1, count1.FromDate);
            Assert.Equal(date2, count1.ToDate);

            var count2 = counts[1];
            Assert.Equal(date2, count2.FromDate);
            Assert.Equal(date3, count2.ToDate);

        }

        [Fact]
        public async Task PhotosGetExifAsyncTest()
        {
            Flickr f = Instance;

            var w = new AsyncSubject<FlickrResult<ExifTagCollection>>();
            var r = await f.PhotosGetExifAsync(TestData.PhotoId);
             { w.OnNext(r); w.OnCompleted(); };
            var result = w.Next().First();

            Assert.False(result.HasError);

        }

        [Fact]
        public async Task PhotosGetRecentAsyncTest()
        {
            Flickr f = Instance;
            var w = new AsyncSubject<FlickrResult<PhotoCollection>>();
            var r = await f.PhotosGetRecentAsync(1, 50, PhotoSearchExtras.All);
             { w.OnNext(r); w.OnCompleted(); };
            var result = w.Next().First();

            Assert.False(result.HasError);
            Assert.NotNull(result.Result);

            Assert.True(result.Result.Count > 0);// "Should return some photos.");

        }


    }
}
