using System;

using Xunit;
using FlickrNet;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for PhotosCommentsGetListTests
    /// </summary>
    
    public class PhotosCommentsTests : BaseTest
    {
        [Fact]
        public void PhotosCommentsGetListBasicTest()
        {
            Flickr f = Instance;

            PhotoCommentCollection comments = f.PhotosCommentsGetList("3546335765");

            Assert.NotNull(comments);//, "PhotoCommentCollection should not be null."

            Assert.Equal(1, comments.Count);//, "Count should be one."

            Assert.Equal("ian1001", comments[0].AuthorUserName);
            Assert.Equal("Sam lucky you NYCis so cool can't wait to go again it's my fav city along with San fran", comments[0].CommentHtml);
        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public void PhotosCommentsGetRecentForContactsBasicTest()
        {
            Flickr f = AuthInstance;

            var photos = f.PhotosCommentsGetRecentForContacts();
            Assert.NotNull(photos);//, "PhotoCollection should not be null."
        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public void PhotosCommentsGetRecentForContactsFullParamTest()
        {
            Flickr f = AuthInstance;

            var photos = f.PhotosCommentsGetRecentForContacts(DateTime.Now.AddHours(-1), PhotoSearchExtras.All, 1, 20);
            Assert.NotNull(photos);//, "PhotoCollection should not be null."
            Assert.Equal(20, photos.PerPage);
        }
    }
}
