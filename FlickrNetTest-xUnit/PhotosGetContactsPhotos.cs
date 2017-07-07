using System;
using FlickrNet;
using Xunit;
using Shouldly;

namespace FlickrNetTest
{
    
    [Trait("Category","AccessTokenRequired")]
    public class PhotosGetContactsPhotos : BaseTest
    {
        [Fact]
        public void PhotosGetContactsPhotosSignatureRequiredTest()
        {
            Should.Throw<SignatureRequiredException>(() => Instance.PhotosGetContactsPhotos());
        }

        [Fact]
        public void PhotosGetContactsPhotosIncorrectCountTest()
        {
            Should.Throw<ArgumentOutOfRangeException>(() => AuthInstance.PhotosGetContactsPhotos(51));
        }

        [Fact]
        public void PhotosGetContactsPhotosBasicTest()
        {
            PhotoCollection photos = AuthInstance.PhotosGetContactsPhotos(10);

            Assert.True(photos.Count > 0, "Should return some photos");
            Assert.Equal(10, photos.Count);//, "Should return 10 photos"

        }

        [Fact]
        public void PhotosGetContactsPhotosExtrasTest()
        {
            PhotoCollection photos = AuthInstance.PhotosGetContactsPhotos(10, false, false, false, PhotoSearchExtras.All);

            Assert.True(photos.Count > 0, "Should return some photos");
            Assert.Equal(10, photos.Count);//, "Should return 10 photos"

            foreach (Photo p in photos)
            {
                Assert.NotNull(p.OwnerName);//, "OwnerName should not be null"
                Assert.NotEqual(default(DateTime), p.DateTaken);//, "DateTaken should not be default DateTime"
            }
        }
    }
}
