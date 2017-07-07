
using Xunit;
using FlickrNet;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for GeoTests
    /// </summary>
    
    public class GeoTests : BaseTest
    {
       
        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public void PhotosGeoGetPermsBasicTest()
        {
            GeoPermissions perms = AuthInstance.PhotosGeoGetPerms(TestData.PhotoId);

            Assert.NotNull(perms);
            Assert.Equal(TestData.PhotoId, perms.PhotoId);
            Assert.True(perms.IsPublic, "IsPublic should be true.");
        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public void PhotosGetWithGeoDataBasicTest()
        {
            PhotoCollection photos = AuthInstance.PhotosGetWithGeoData();

            Assert.NotNull(photos);
            Assert.NotEqual(0, photos.Count);
            Assert.NotEqual(0, photos.Total);
            Assert.Equal(1, photos.Page);
            Assert.NotEqual(0, photos.PerPage);
            Assert.NotEqual(0, photos.Pages);

            foreach (var p in photos)
            {
                Assert.NotNull(p.PhotoId);
            }

        }
    }
}
