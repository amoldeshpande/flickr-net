using System;

using Xunit;
using FlickrNet;

namespace FlickrNetTest
{
    [Trait("Category","AccessTokenRequired")]
    public class StatsGetPopularPhotosTests : BaseTest
    {
        [Fact]
        public void StatsGetPopularPhotosBasic()
        {
            PopularPhotoCollection photos = AuthInstance.StatsGetPopularPhotos(DateTime.MinValue, PopularitySort.None, 0, 0);

            Assert.NotNull(photos);//, "PopularPhotos should not be null."

            Assert.NotEqual(0, photos.Total);//, "PopularPhotos.Total should not be zero."
            Assert.NotEqual(0, photos.Count);//, "PopularPhotos.Count should not be zero."
            Assert.Equal(photos.Count, Math.Min(photos.Total, photos.PerPage));//, "PopularPhotos.Count should equal either PopularPhotos.Total or PopularPhotos.PerPage."

            foreach (Photo p in photos)
            {
                Assert.NotNull(p.PhotoId);//, "Photo.PhotoId should not be null."
            }

            foreach (PopularPhoto p in photos)
            {
                Assert.NotNull(p.PhotoId);//, "PopularPhoto.PhotoId should not be null."
                Assert.NotEqual(0, p.StatViews);//, "PopularPhoto.StatViews should not be zero."
            }
        }

        [Fact]
        public void StatsGetPopularPhotosNoParamsTest()
        {
            Flickr f = AuthInstance;

            PopularPhotoCollection photos = f.StatsGetPopularPhotos();

            Assert.NotNull(photos);//, "PopularPhotos should not be null."

            Assert.NotEqual(0, photos.Total);//, "PopularPhotos.Total should not be zero."
            Assert.NotEqual(0, photos.Count);//, "PopularPhotos.Count should not be zero."
            Assert.Equal(photos.Count, Math.Min(photos.Total, photos.PerPage));//, "PopularPhotos.Count should equal either PopularPhotos.Total or PopularPhotos.PerPage."

            foreach (Photo p in photos)
            {
                Assert.NotNull(p.PhotoId);//, "Photo.PhotoId should not be null."
            }

            foreach (PopularPhoto p in photos)
            {
                Assert.NotNull(p.PhotoId);//, "PopularPhoto.PhotoId should not be null."
                Assert.NotEqual(0, p.StatViews);//, "PopularPhoto.StatViews should not be zero."
            }
        }

        [Fact]
        public void StatsGetPopularPhotosOtherTest()
        {
            var lastWeek = DateTime.Today.AddDays(-7);

            var photos = AuthInstance.StatsGetPopularPhotos(lastWeek);
            Assert.NotNull(photos);//, "PopularPhotos should not be null."

            photos = AuthInstance.StatsGetPopularPhotos(PopularitySort.Favorites);
            Assert.NotNull(photos);//, "PopularPhotos should not be null."

            photos = AuthInstance.StatsGetPopularPhotos(lastWeek, 1, 10);
            Assert.NotNull(photos);//, "PopularPhotos should not be null."
            Assert.Equal(10, photos.Count);//, "Date search popular photos should return 10 photos."

            photos = AuthInstance.StatsGetPopularPhotos(PopularitySort.Favorites, 1, 10);
            Assert.NotNull(photos);//, "PopularPhotos should not be null."
            Assert.Equal(10, photos.Count);//, "Favorite search popular photos should return 10 photos."

        }
    }
}
