using System.Linq;
using NUnit.Framework;
using FlickrNet;
using System.Threading.Tasks;
using System.Runtime.ExceptionServices;
using System;

namespace FlickrNetTest.Async
{
    [TestFixture]
    public class PhotosetsAsyncTests : BaseTest
    {
        [Test]
        public async Task PhotosetsGetContextAsyncTest()
        {
            Flickr f = Instance;

            var photosetId = "72157626420254033"; // Beamish
            var photos = f.PhotosetsGetPhotos(photosetId, 1, 100);
            var firstPhoto = photos.First();
            var lastPhoto = photos.Last();


            var result = await f.PhotosetsGetContextAsync(firstPhoto.PhotoId, photosetId);

            Assert.IsFalse(result.HasError);

            var context = result.Result;

            Assert.IsNull(context.PreviousPhoto, "As this is the first photo the previous photo should be null.");
            Assert.IsNotNull(context.NextPhoto, "As this is the first photo the next photo should not be null.");


            result = await f.PhotosetsGetContextAsync(lastPhoto.PhotoId, photosetId);

            Assert.IsFalse(result.HasError);

            context = result.Result;

            Assert.IsNull(context.NextPhoto, "As this is the last photo the next photo should be null.");
            Assert.IsNotNull(context.PreviousPhoto, "As this is the last photo the previous photo should not be null.");
        }

        [Test]
        public async Task PhotosetsGetInfoAsyncTest()
        {
            Flickr f = Instance;

            var photoset = f.PhotosetsGetList(TestData.TestUserId).First();

            var result = await f.PhotosetsGetInfoAsync(photoset.PhotosetId);
        }

        [Test]
        [Category("AccessTokenRequired")]
        public async Task PhotosetsGeneralAsyncTest()
        {
            Flickr f = AuthInstance;
            ExceptionDispatchInfo cap = null;

            var photoId1 = "7519320006"; // Tree/Write/Wall
            var photoId2 = "7176125763"; // Rainbow Rose

            var photosetResult = await f.PhotosetsCreateAsync("Test Photoset", photoId1);

            Assert.IsFalse(photosetResult.HasError);
            var photoset = photosetResult.Result;


            try
            {
                var noResponseResult = await f.PhotosetsEditMetaAsync(photoset.PhotosetId, "New Title", "New Description");
                Assert.IsFalse(noResponseResult.HasError);

                noResponseResult = await f.PhotosetsAddPhotoAsync(photoset.PhotosetId, photoId2);

                Assert.IsFalse(noResponseResult.HasError);
            }
            catch(Exception ex)
            {
                cap = ExceptionDispatchInfo.Capture(ex);
            }
            {
                // Clean up and delete photoset
                var noResponseResult = await f.PhotosetsDeleteAsync(photoset.PhotosetId);
            }
            if (cap != null)
            {
                cap.Throw();
            }
        }

        [Test]
        public async Task PhotosetsGetPhotosAsyncTest()
        {
            var photoset = Instance.PhotosetsGetList(TestData.TestUserId).First();

            var result = await Instance.PhotosetsGetPhotosAsync(photoset.PhotosetId, PhotoSearchExtras.All, PrivacyFilter.PublicPhotos, 1, 50, MediaType.All);

            Assert.IsFalse(result.HasError);
            
        }
    }
}
