using System.Linq;
using Xunit;

namespace FlickrNetTest
{
    [Trait("Category","AccessTokenRequired")]
    public class PhotosetsOrderSets : BaseTest
    {
        [Fact]
        public void PhotosetsOrderSetsStringTest()
        {
            var mySets = AuthInstance.PhotosetsGetList();

            AuthInstance.PhotosetsOrderSets(string.Join(",", mySets.Select(myset => myset.PhotosetId).ToArray()));

        }

        [Fact]
        public void PhotosetsOrderSetsArrayTest()
        {
            var mySets = AuthInstance.PhotosetsGetList();

            AuthInstance.PhotosetsOrderSets(mySets.Select(myset => myset.PhotosetId).ToArray());
        }
    }
}
