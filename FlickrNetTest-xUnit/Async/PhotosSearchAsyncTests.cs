
using Xunit;
using FlickrNet;
using System.Linq;
using System.Reactive.Subjects;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for PhotosSearchAsyncTests
    /// </summary>
    
    public class PhotosSearchAsyncTests : BaseTest
    {

        [Fact]
        public async Task PhotosSearchAsyncBasicTest()
        {
            var o = new PhotoSearchOptions();
            o.Tags = "microsoft";

            var w = new AsyncSubject<FlickrResult<PhotoCollection>>();

            var r = await Instance.PhotosSearchAsync(o);
             { w.OnNext(r); w.OnCompleted(); };
            var result = w.Next().First();

            Assert.True(result.Result.Total > 0);

        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]

        public async Task PhotosAddTagTest()
        {
            string photoId = "4499284951";
            string tag = "testx";

            var w = new AsyncSubject<FlickrResult<NoResponse>>();

            var r = await AuthInstance.PhotosAddTagsAsync(photoId, tag);
             { w.OnNext(r); w.OnCompleted(); };

            var result = w.Next().First();
            
        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]

        public async Task PhotosSearchAsyncShowerTest()
        {
            var o = new PhotoSearchOptions();
            o.UserId = "78507951@N00";
            o.Tags = "shower";
            o.SortOrder = PhotoSearchSortOrder.DatePostedDescending;
            o.PerPage = 1000;
            o.TagMode = TagMode.AllTags;
            o.Extras = PhotoSearchExtras.All;

            var w = new AsyncSubject<FlickrResult<PhotoCollection>>();

            var r = await AuthInstance.PhotosSearchAsync(o);
             { w.OnNext(r); w.OnCompleted(); };
            var result = w.Next().First();

            Assert.True(result.Result.Total > 0);
        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]

        public async Task PhotosGetContactsPhotosAsyncTest()
        {
            var w = new AsyncSubject<FlickrResult<PhotoCollection>>();
            var r = await AuthInstance.PhotosGetContactsPhotosAsync(50, true, true, true, PhotoSearchExtras.All);
             { w.OnNext(r); w.OnCompleted(); };
            var result = w.Next().First();

            Assert.False(result.HasError);
            Assert.NotNull(result.Result);

            Assert.True(result.Result.Count > 0, "Should return some photos.");

        }


    }
}
