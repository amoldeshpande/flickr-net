using System;
using System.Linq;
using NUnit.Framework;
using FlickrNet;
using Shouldly;
using System.Threading.Tasks;

namespace FlickrNetTest.Async
{
    [TestFixture]
    public class PhotosAsyncTests : BaseTest
    {
        [Test]
        public async Task PhotosSearchRussianAsync()
        {
            var o = new PhotoSearchOptions();
            o.Extras = PhotoSearchExtras.Tags;
            o.Tags = "фото";
            o.PerPage = 100;

            Flickr f = Instance;

            var result = await f.PhotosSearchAsync(o);

            Assert.IsFalse(result.HasError);
            Assert.IsNotNull(result.Result);

            result.Result.Count.ShouldBeGreaterThan(0);

            var photos = result.Result;
            foreach (var photo in photos)
            {
                Console.WriteLine(photo.Title + " = " + string.Join(",", photo.Tags));
            }

        }
        [Test]
        public async Task PhotosGetContactsPublicPhotosAsyncTest()
        {
            Flickr f = Instance;

            var result = await f.PhotosGetContactsPublicPhotosAsync(TestData.TestUserId, 5, true, true, true, PhotoSearchExtras.All);

            Assert.IsFalse(result.HasError);
            Assert.IsNotNull(result.Result);

            Assert.IsTrue(result.Result.Count > 0, "Should return some photos.");
        }

        [Test]
        [Category("AccessTokenRequired")]
        public async Task PhotosGetCountsAsyncTest()
        {
            DateTime date1 = DateTime.Today.AddMonths(-12);
            DateTime date2 = DateTime.Today.AddMonths(-6);
            DateTime date3 = DateTime.Today;

            DateTime[] uploadDates = { date1, date2, date3 };

            Flickr f = AuthInstance;

            var result = await f.PhotosGetCountsAsync(uploadDates, false);

            Assert.IsFalse(result.HasError);

            var counts = result.Result;

            Assert.AreEqual(2, counts.Count, "Should be two counts returned.");

            var count1 = counts[0];

            Assert.AreEqual(date1, count1.FromDate);
            Assert.AreEqual(date2, count1.ToDate);

            var count2 = counts[1];
            Assert.AreEqual(date2, count2.FromDate);
            Assert.AreEqual(date3, count2.ToDate);

        }

        [Test]
        public async Task PhotosGetExifAsyncTest()
        {
            Flickr f = Instance;

            var result = await f.PhotosGetExifAsync(TestData.PhotoId);

            Assert.IsFalse(result.HasError);

        }

        [Test]
        public async Task PhotosGetRecentAsyncTest()
        {
            Flickr f = Instance;
            var result = await f.PhotosGetRecentAsync(1, 50, PhotoSearchExtras.All);

            Assert.IsFalse(result.HasError);
            Assert.IsNotNull(result.Result);

            Assert.IsTrue(result.Result.Count > 0, "Should return some photos.");

        }


    }
}
