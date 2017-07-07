
using Xunit;
using FlickrNet;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for PhotoOwnerNameTest
    /// </summary>
    public class PhotoOwnerNameTest : BaseTest
    {
        [Fact]
        public void PhotosSearchOwnerNameTest()
        {
            var o = new PhotoSearchOptions();

            o.UserId = TestData.TestUserId;
            o.PerPage = 10;
            o.Extras = PhotoSearchExtras.OwnerName;

            Flickr f = Instance;
            PhotoCollection photos = f.PhotosSearch(o);

            Assert.NotNull(photos[0].OwnerName);
           
        }

        [Fact]
        public void PhotosGetContactsPublicPhotosOwnerNameTest()
        {
            Flickr f = Instance;
            PhotoCollection photos = f.PhotosGetContactsPublicPhotos(TestData.TestUserId, PhotoSearchExtras.OwnerName);

            Assert.NotNull(photos[0].OwnerName);
        }

    }
}
