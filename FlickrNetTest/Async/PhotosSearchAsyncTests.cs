
using NUnit.Framework;
using FlickrNet;
using System.Linq;
using System.Threading.Tasks;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for PhotosSearchAsyncTests
    /// </summary>
    public class PhotosSearchAsyncTests : BaseTest
    {

        [Test]
        public async Task PhotosSearchAsyncBasicTest()
        {
            var o = new PhotoSearchOptions();
            o.Tags = "microsoft";

            var result = await Instance.PhotosSearchAsync(o);

            Assert.IsTrue(result.Result.Total > 0);

        }

        [Test]
        [Category("AccessTokenRequired")]

        public async Task PhotosAddTagTest()
        {
            string photoId = "4499284951";
            string tag = "testx";

            var result = await AuthInstance.PhotosAddTagsAsync(photoId, tag);
            
        }

        [Test]
        [Category("AccessTokenRequired")]
        public async Task PhotosSearchAsyncShowerTest()
        {
            var o = new PhotoSearchOptions();
            o.UserId = "78507951@N00";
            o.Tags = "shower";
            o.SortOrder = PhotoSearchSortOrder.DatePostedDescending;
            o.PerPage = 1000;
            o.TagMode = TagMode.AllTags;
            o.Extras = PhotoSearchExtras.All;

            var result = await AuthInstance.PhotosSearchAsync(o);

            Assert.IsTrue(result.Result.Total > 0);
        }

        [Test]
        [Category("AccessTokenRequired")]
        public async Task PhotosGetContactsPhotosAsyncTest()
        {
            var result = await AuthInstance.PhotosGetContactsPhotosAsync(50, true, true, true, PhotoSearchExtras.All);

            Assert.IsFalse(result.HasError);
            Assert.IsNotNull(result.Result);

            Assert.IsTrue(result.Result.Count > 0, "Should return some photos.");

        }


    }
}
