using System;
using System.Linq;

using Xunit;
using FlickrNet;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for FavouritesGetPublicListTests
    /// </summary>
    
    public class FavoritesTests : BaseTest
    {
        [Fact]
        public void FavoritesGetPublicListBasicTest()
        {
            const string userId = "77788903@N00";

            var p = Instance.FavoritesGetPublicList(userId);

            Assert.NotNull(p);//, "PhotoCollection should not be null instance."
            Assert.NotEqual(0, p.Count);//, "PhotoCollection.Count should be greater than zero."
        }

        [Fact]
        public void FavouritesGetPublicListWithDates()
        {
            var allFavourites = Instance.FavoritesGetPublicList(TestData.TestUserId);

            var firstFiveFavourites = allFavourites.OrderBy(p => p.DateFavorited).Take(5).ToList();

            var minDate = firstFiveFavourites.Min(p => p.DateFavorited).GetValueOrDefault();
            var maxDate = firstFiveFavourites.Max(p => p.DateFavorited).GetValueOrDefault();

            var subsetOfFavourites = Instance.FavoritesGetPublicList(TestData.TestUserId, minDate, maxDate,
                                                                     PhotoSearchExtras.None, 0, 0);

            Assert.Equal(5, subsetOfFavourites.Count);//, "Should be 5 favourites in subset"
        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public void FavoritesGetListBasicTest()
        {
            var photos = AuthInstance.FavoritesGetList();
            Assert.NotNull(photos);//, "PhotoCollection should not be null instance."
            Assert.NotEqual(0, photos.Count);//, "PhotoCollection.Count should be greater than zero."
        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public void FavoritesGetListFullParamTest()
        {
            var photos = AuthInstance.FavoritesGetList(TestData.TestUserId, DateTime.Now.AddYears(-2), DateTime.Now, PhotoSearchExtras.All, 1, 10);
            Assert.NotNull(photos);//, "PhotoCollection should not be null."

            Assert.True(photos.Count > 0, "Count should be greater than zero.");

        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public void FavoritesGetListPartialParamTest()
        {
            PhotoCollection photos = AuthInstance.FavoritesGetList(TestData.TestUserId, 2, 20);
            Assert.NotNull(photos);//, "PhotoCollection should not be null instance."
            Assert.NotEqual(0, photos.Count);//, "PhotoCollection.Count should be greater than zero."
            Assert.Equal(2, photos.Page);
            Assert.Equal(20, photos.PerPage);
            Assert.Equal(20, photos.Count);
        }

        [Fact]
        public void FavoritesGetContext()
        {
            const string photoId = "2502963121";
            const string userId = "41888973@N00";

            var context = Instance.FavoritesGetContext(photoId, userId);

            Assert.NotNull(context);
            Assert.NotEqual(0, context.Count);//, "Count should be greater than zero"
            Assert.Equal(1, context.PreviousPhotos.Count);//, "Should be 1 previous photo."
            Assert.Equal(1, context.NextPhotos.Count);//, "Should be 1 next photo."
        }

        [Fact]
        public void FavoritesGetContextMorePrevious()
        {
            const string photoId = "2502963121";
            const string userId = "41888973@N00";

            var context = Instance.FavoritesGetContext(photoId, userId, 3, 4, PhotoSearchExtras.Description);

            Assert.NotNull(context);
            Assert.NotEqual(0, context.Count);//, "Count should be greater than zero"
            Assert.Equal(3, context.PreviousPhotos.Count);//, "Should be 3 previous photo."
            Assert.Equal(4, context.NextPhotos.Count);//, "Should be 4 next photo."
        }

    }
}
