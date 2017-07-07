
using Xunit;
using FlickrNet;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for UrlsTests
    /// </summary>
    
    public class UrlsTests : BaseTest
    {
        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public void UrlsLookupUserTest1()
        {
            FoundUser user = AuthInstance.UrlsLookupUser("https://www.flickr.com/photos/samjudson");

            Assert.Equal("41888973@N00", user.UserId);
            Assert.Equal("Sam Judson", user.UserName);
        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public void UrlsLookupGroup()
        {
            string groupUrl = "https://www.flickr.com/groups/angels_of_the_north/";

            string groupId = AuthInstance.UrlsLookupGroup(groupUrl);

            Assert.Equal("71585219@N00", groupId);
        }

        [Fact]
        public void UrlsLookupGalleryTest()
        {
            string galleryUrl = "https://www.flickr.com/photos/samjudson/galleries/72157622589312064";

            Flickr f = Instance;

            Gallery gallery = f.UrlsLookupGallery(galleryUrl);

            Assert.Equal(galleryUrl, gallery.GalleryUrl);

        }

        [Fact]
        public void UrlsGetUserPhotosTest()
        {
            string url = Instance.UrlsGetUserPhotos(TestData.TestUserId);

            Assert.Equal("https://www.flickr.com/photos/samjudson/", url);
        }

        [Fact]
        public void UrlsGetUserProfileTest()
        {
            string url = Instance.UrlsGetUserProfile(TestData.TestUserId);

            Assert.Equal("https://www.flickr.com/people/samjudson/", url);
        }

        [Fact]
        public void UrlsGetGroupTest()
        {
            string url = Instance.UrlsGetGroup(TestData.GroupId);

            Assert.Equal("https://www.flickr.com/groups/lakedistrict/", url);
        }



    }
}
