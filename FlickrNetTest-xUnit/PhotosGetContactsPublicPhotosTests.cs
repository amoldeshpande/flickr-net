using FlickrNet;
using Xunit;

namespace FlickrNetTest
{

    public class PhotosGetContactsPublicPhotosTests : BaseTest
    {

        [Fact]
        public void PhotosGetContactsPublicPhotosUserIdExtrasTest()
        {
            Flickr f = Instance;

            string userId = TestData.TestUserId;
            PhotoSearchExtras extras = PhotoSearchExtras.All;
            var photos = f.PhotosGetContactsPublicPhotos(userId, extras);

            Assert.NotNull(photos);
            Assert.NotEqual(0, photos.Count);//, "Should have returned more than 0 photos"
        }

        [Fact]
        public void PhotosGetContactsPublicPhotosAllParamsTest()
        {
            Flickr f = Instance;

            string userId = TestData.TestUserId;

            int count = 4; // TODO: Initialize to an appropriate value
            bool justFriends = true; // TODO: Initialize to an appropriate value
            bool singlePhoto = true; // TODO: Initialize to an appropriate value
            bool includeSelf = false; // TODO: Initialize to an appropriate value
            PhotoSearchExtras extras = PhotoSearchExtras.None;

            var photos = f.PhotosGetContactsPublicPhotos(userId, count, justFriends, singlePhoto, includeSelf, extras);

            Assert.NotNull(photos);
            Assert.NotEqual(0, photos.Count);//, "Should have returned more than 0 photos"
        }

        [Fact]
        public void PhotosGetContactsPublicPhotosExceptExtrasTest()
        {
            Flickr f = Instance;

            string userId = TestData.TestUserId;

            int count = 4; 
            bool justFriends = true; 
            bool singlePhoto = true; 
            bool includeSelf = false; 

            var photos = f.PhotosGetContactsPublicPhotos(userId, count, justFriends, singlePhoto, includeSelf);

            Assert.NotNull(photos);
            Assert.NotEqual(0, photos.Count);//, "Should have returned more than 0 photos"
        }

        [Fact]
        public void PhotosGetContactsPublicPhotosUserIdTest()
        {
            Flickr f = Instance;

            string userId = TestData.TestUserId;

            var photos = f.PhotosGetContactsPublicPhotos(userId);

            Assert.NotNull(photos);
            Assert.NotEqual(0, photos.Count);//, "Should have returned more than 0 photos"
        }

        [Fact]
        public void PhotosGetContactsPublicPhotosUserIdCountExtrasTest()
        {
            Flickr f = Instance;

            string userId = TestData.TestUserId;

            int count = 5; 
            PhotoSearchExtras extras = PhotoSearchExtras.None;

            var photos = f.PhotosGetContactsPublicPhotos(userId, count, extras);

            Assert.NotNull(photos);
            Assert.NotEqual(0, photos.Count);//, "Should have returned more than 0 photos"
        }

        [Fact]
        public void PhotosGetContactsPublicPhotosUserIdCountTest()
        {
            Flickr f = Instance;

            string userId = TestData.TestUserId;

            int count = 5;

            var photos = f.PhotosGetContactsPublicPhotos(userId, count);

            Assert.NotNull(photos);
            Assert.NotEqual(0, photos.Count);//, "Should have returned more than 0 photos"
        }
    }
}
