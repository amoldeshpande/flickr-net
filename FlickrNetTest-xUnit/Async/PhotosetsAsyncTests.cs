using System.Linq;
using Xunit;
using FlickrNet;
using System.Threading.Tasks;
using System.Runtime.ExceptionServices;
using System;
using System.Reactive.Subjects;
using System.Reactive.Linq;

namespace FlickrNetTest.Async
{
    public class PhotosetsAsyncTests : BaseTest
    {
        [Fact]
        public async Task PhotosetsGetContextAsyncTest()
        {
            Flickr f = Instance;

            var photosetId = "72157626420254033"; // Beamish
            var photos = f.PhotosetsGetPhotos(photosetId, 1, 100);
            var firstPhoto = photos.First();
            var lastPhoto = photos.Last();

            var w = new AsyncSubject<FlickrResult<Context>>();

            var r = await f.PhotosetsGetContextAsync(firstPhoto.PhotoId, photosetId);
             { w.OnNext(r); w.OnCompleted(); };
            var result = w.Next().First();

            Assert.False(result.HasError);

            var context = result.Result;

            Assert.Null(context.PreviousPhoto);// "As this is the first photo the previous photo should be null.");
            Assert.NotNull(context.NextPhoto);// "As this is the first photo the next photo should not be null.");

            w = new AsyncSubject<FlickrResult<Context>>();

            var r2 = await f.PhotosetsGetContextAsync(lastPhoto.PhotoId, photosetId);
             { w.OnNext(r2); w.OnCompleted(); };
            result = w.Next().First();

            Assert.False(result.HasError);

            context = result.Result;

            Assert.Null(context.NextPhoto);// "As this is the last photo the next photo should be null.");
            Assert.NotNull(context.PreviousPhoto);// "As this is the last photo the previous photo should not be null.");
        }

        [Fact]
        public async Task PhotosetsGetInfoAsyncTest()
        {
            Flickr f = Instance;

            var photoset = f.PhotosetsGetList(TestData.TestUserId).First();

            var w = new AsyncSubject<FlickrResult<Photoset>>();

            var r = await f.PhotosetsGetInfoAsync(photoset.PhotosetId);
             { w.OnNext(r); w.OnCompleted(); };
            var result = w.Next().First();
        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public async Task PhotosetsGeneralAsyncTest()
        {
            Flickr f = AuthInstance;
            ExceptionDispatchInfo cap = null;

            var photoId1 = "7519320006"; // Tree/Write/Wall
            var photoId2 = "7176125763"; // Rainbow Rose

            var w = new AsyncSubject<FlickrResult<Photoset>>();
            var r = await f.PhotosetsCreateAsync("Test Photoset", photoId1);
            { w.OnNext(r); w.OnCompleted(); };

            var photosetResult = w.Next().First();
            Assert.False(photosetResult.HasError);
            var photoset = photosetResult.Result;


            try
            {
                var w2 = new AsyncSubject<FlickrResult<NoResponse>>();
                var r2 = await f.PhotosetsEditMetaAsync(photoset.PhotosetId, "New Title", "New Description");
                { w2.OnNext(r2); w2.OnCompleted(); };
                var noResponseResult2 = w2.Next().First();
                Assert.False(noResponseResult2.HasError);

                var w3 = new AsyncSubject<FlickrResult<NoResponse>>();
                var r3 = await f.PhotosetsAddPhotoAsync(photoset.PhotosetId, photoId2);
                { w3.OnNext(r3); w3.OnCompleted(); };

                var noResponseResult3 = w3.Next().First();
                Assert.False(noResponseResult3.HasError);
            }
            catch (Exception ex)
            {
                cap = ExceptionDispatchInfo.Capture(ex);
            }

            var w4 = new AsyncSubject<FlickrResult<NoResponse>>();
            // Clean up and delete photoset
            var r4 = await f.PhotosetsDeleteAsync(photoset.PhotosetId);
            { w4.OnNext(r4); w4.OnCompleted(); };
            var noResponseResult4 = w4.Next().First();
            
            if (cap != null)
            {
                cap.Throw();
            }

        }

        [Fact]
        public async Task PhotosetsGetPhotosAsyncTest()
        {
            var photoset = Instance.PhotosetsGetList(TestData.TestUserId).First();

            var w = new AsyncSubject<FlickrResult<PhotosetPhotoCollection>>();

            var r = await Instance.PhotosetsGetPhotosAsync(photoset.PhotosetId, PhotoSearchExtras.All, PrivacyFilter.PublicPhotos, 1, 50, MediaType.All);
             { w.OnNext(r); w.OnCompleted(); };
            var result = w.Next().First();

            Assert.False(result.HasError);
            
        }
    }
}
