
using Xunit;
using FlickrNet;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for PhotosetCommentsGetListTests
    /// </summary>
    
    public class PhotosetCommentsGetListTests : BaseTest
    {
       [Fact]
        public void PhotosetsCommentsGetListBasicTest()
        {
            Flickr f = Instance;

            PhotosetCommentCollection comments = f.PhotosetsCommentsGetList("1335934");

            Assert.NotNull(comments);

            Assert.Equal(2, comments.Count);

            Assert.Equal("Superchou", comments[0].AuthorUserName);
            Assert.Equal("LOL... I had no idea this set existed... what a great afternoon we had :)", comments[0].CommentHtml);
        }
    }
}
