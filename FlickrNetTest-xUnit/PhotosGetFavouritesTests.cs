using System;
using FlickrNet;
using Xunit;

namespace FlickrNetTest
{
    
    public class PhotosGetFavouritesTests : BaseTest
    {
        [Fact]
        public void PhotosGetFavoritesNoFavourites()
        {
            // No favourites
            PhotoFavoriteCollection favs = Instance.PhotosGetFavorites(TestData.PhotoId);

            Assert.Equal(0, favs.Count);//, "Should have no favourites"

        }

        [Fact]
        public void PhotosGetFavoritesHasFavourites()
        {
            PhotoFavoriteCollection favs = Instance.PhotosGetFavorites(TestData.FavouritedPhotoId, 500, 1);

            Assert.NotNull(favs);

            Assert.True(favs.Count > 0, "PhotoFavourites.Count should not be zero.");

            Assert.Equal(50, favs.Count);//, "Should be 50 favourites listed (maximum returned)"

            foreach (PhotoFavorite p in favs)
            {
                Assert.False(string.IsNullOrEmpty(p.UserId), "Should have a user ID.");
                Assert.False(string.IsNullOrEmpty(p.UserName), "Should have a user name.");
                Assert.NotEqual(default(DateTime), p.FavoriteDate);//, "Favourite Date should not be default Date value"
                Assert.True(p.FavoriteDate < DateTime.Now, "Favourite Date should be in the past.");
            }
        }

        [Fact]
        public void PhotosGetFavoritesPaging()
        {
            PhotoFavoriteCollection favs = Instance.PhotosGetFavorites(TestData.FavouritedPhotoId, 10, 1);

            Assert.Equal(10, favs.Count);//, "PhotoFavourites.Count should be 10."
            Assert.Equal(10, favs.PerPage);//, "PhotoFavourites.PerPage should be 10"
            Assert.Equal(1, favs.Page);//, "PhotoFavourites.Page should be 1."
            Assert.True(favs.Total > 100, "PhotoFavourites.Total should be greater than 100.");
            Assert.True(favs.Pages > 10, "PhotoFavourites.Pages should be greater than 10.");
        }

        [Fact]
        public void PhotosGetFavoritesPagingTwo()
        {
            PhotoFavoriteCollection favs = Instance.PhotosGetFavorites(TestData.FavouritedPhotoId, 10, 2);

            Assert.Equal(10, favs.Count);//, "PhotoFavourites.Count should be 10."
            Assert.Equal(10, favs.PerPage);//, "PhotoFavourites.PerPage should be 10"
            Assert.Equal(2, favs.Page);//, "PhotoFavourites.Page should be 2."
        }
    }
}
