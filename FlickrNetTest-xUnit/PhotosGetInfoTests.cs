using System;
using System.Linq;
using FlickrNet.Exceptions;
using Xunit;
using FlickrNet;
using System.Reactive.Subjects;
using System.Reactive.Linq;
using Shouldly;
using System.Threading.Tasks;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for PhotosGetInfoTests
    /// </summary>
    
    public class PhotosGetInfoTests : BaseTest
    {
        [Fact]
        [Trait("Category","AccessTokenRequired")]

        public void PhotosGetInfoBasicTest()
        {
            PhotoInfo info = AuthInstance.PhotosGetInfo("4268023123");

            Assert.NotNull(info);

            Assert.Equal("4268023123", info.PhotoId);
            Assert.Equal("a4283bac01", info.Secret);
            Assert.Equal("2795", info.Server);
            Assert.Equal("3", info.Farm);
            Assert.Equal(UtilityMethods.UnixTimestampToDate("1263291891"), info.DateUploaded);
            Assert.False(info.IsFavorite);
            Assert.Equal(LicenseType.AttributionNoncommercialShareAlikeCC, info.License);
            Assert.Equal(0, info.Rotation);
            Assert.Equal("9d3d4bf24a", info.OriginalSecret);
            Assert.Equal("jpg", info.OriginalFormat);
            Assert.True(info.ViewCount > 87);// "ViewCount should be greater than 87.");
            Assert.Equal(MediaType.Photos, info.Media);

            Assert.Equal("12. Sudoku", info.Title);
            Assert.Equal("It scares me sometimes how much some of my handwriting reminds me of Dad's " +
                            "- in this photo there is one 5 that especially reminds me of his handwriting.", info.Description);

            //Owner
            Assert.Equal("41888973@N00", info.OwnerUserId);

            //Dates
            Assert.Equal(new DateTime(2010, 01, 12, 11, 01, 20), info.DateTaken);//, "DateTaken is not set correctly."

            //Editability
            Assert.True(info.CanComment, "CanComment should be true when authenticated.");
            Assert.True(info.CanAddMeta, "CanAddMeta should be true when authenticated.");

            //Permissions
            Assert.Equal(PermissionComment.Everybody, info.PermissionComment);
            Assert.Equal(PermissionAddMeta.Everybody, info.PermissionAddMeta);

            //Visibility

            // Notes

            Assert.Equal(1, info.Notes.Count);//, "Notes.Count should be one."
            Assert.Equal("72157623069944527", info.Notes[0].NoteId);
            Assert.Equal("41888973@N00", info.Notes[0].AuthorId);
            Assert.Equal("Sam Judson", info.Notes[0].AuthorName);
            Assert.Equal(267, info.Notes[0].XPosition);
            Assert.Equal(238, info.Notes[0].YPosition);

            // Tags

            Assert.Equal(5, info.Tags.Count);
            Assert.Equal("78188-4268023123-586", info.Tags[0].TagId);
            Assert.Equal("green", info.Tags[0].Raw);

            // URLs

            Assert.Equal(1, info.Urls.Count);
            Assert.Equal("photopage", info.Urls[0].UrlType);
            Assert.Equal("https://www.flickr.com/photos/samjudson/4268023123/", info.Urls[0].Url);

        }

        [Fact]
        public void PhotosGetInfoUnauthenticatedTest()
        {
            PhotoInfo info = Instance.PhotosGetInfo("4268023123");

            Assert.NotNull(info);

            Assert.Equal("4268023123", info.PhotoId);
            Assert.Equal("a4283bac01", info.Secret);
            Assert.Equal("2795", info.Server);
            Assert.Equal("3", info.Farm);
            Assert.Equal(UtilityMethods.UnixTimestampToDate("1263291891"), info.DateUploaded);
            Assert.False( info.IsFavorite);
            Assert.Equal(LicenseType.AttributionNoncommercialShareAlikeCC, info.License);
            Assert.Equal(0, info.Rotation);
            Assert.Equal("9d3d4bf24a", info.OriginalSecret);
            Assert.Equal("jpg", info.OriginalFormat);
            Assert.True(info.ViewCount > 87);// "ViewCount should be greater than 87.");
            Assert.Equal(MediaType.Photos, info.Media);

            Assert.Equal("12. Sudoku", info.Title);
            Assert.Equal("It scares me sometimes how much some of my handwriting reminds me of Dad's " +
                            "- in this photo there is one 5 that especially reminds me of his handwriting.", info.Description);

            //Owner
            Assert.Equal("41888973@N00", info.OwnerUserId);

            //Dates

            //Editability
            Assert.False(info.CanComment, "CanComment should be false when not authenticated.");
            Assert.False(info.CanAddMeta, "CanAddMeta should be false when not authenticated.");

            //Permissions
            Assert.Null(info.PermissionComment);// "PermissionComment should be null when not authenticated.");
            Assert.Null(info.PermissionAddMeta);// "PermissionAddMeta should be null when not authenticated.");

            //Visibility

            // Notes

            Assert.Equal(1, info.Notes.Count);//, "Notes.Count should be one."
            Assert.Equal("72157623069944527", info.Notes[0].NoteId);
            Assert.Equal("41888973@N00", info.Notes[0].AuthorId);
            Assert.Equal("Sam Judson", info.Notes[0].AuthorName);
            Assert.Equal(267, info.Notes[0].XPosition);
            Assert.Equal(238, info.Notes[0].YPosition);

            // Tags

            Assert.Equal(5, info.Tags.Count);
            Assert.Equal("78188-4268023123-586", info.Tags[0].TagId);
            Assert.Equal("green", info.Tags[0].Raw);

            // URLs

            Assert.Equal(1, info.Urls.Count);
            Assert.Equal("photopage", info.Urls[0].UrlType);
            Assert.Equal("https://www.flickr.com/photos/samjudson/4268023123/", info.Urls[0].Url);
        }

        [Fact]
        public void PhotosGetInfoTestUserIssue()
        {
            var photoId = "14042679057";
            var info = Instance.PhotosGetInfo(photoId);

            Assert.Equal(photoId, info.PhotoId);
            Assert.Equal("63226137@N02", info.OwnerUserId);
            Assert.Equal("https://www.flickr.com/photos/63226137@N02/14042679057/", info.WebUrl);

        }
        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public void PhotosGetInfoTestLocation()
        {
            const string photoId = "4268756940";

            PhotoInfo info = AuthInstance.PhotosGetInfo(photoId);

            Assert.NotNull(info.Location);
        }

        [Fact]
        public void PhotosGetInfoWithPeople()
        {
            const string photoId = "3547137580"; // https://www.flickr.com/photos/samjudson/3547137580/in/photosof-samjudson/

            PhotoInfo info = Instance.PhotosGetInfo(photoId);

            Assert.NotNull(info);
            Assert.True(info.HasPeople);// "HasPeople should be true.");

        }

        [Fact]
        public void PhotosGetInfoCanBlogTest()
        {
            var o = new PhotoSearchOptions();
            o.UserId = TestData.TestUserId;
            o.PerPage = 5;

            PhotoCollection photos = Instance.PhotosSearch(o);
            PhotoInfo info = Instance.PhotosGetInfo(photos[0].PhotoId);

            Assert.False( info.CanBlog);
            Assert.True( info.CanDownload);
        }

        [Fact]
        public void PhotosGetInfoDataTakenGranularityTest()
        {
            const string photoid = "4386780023";

            PhotoInfo info = Instance.PhotosGetInfo(photoid);

            Assert.Equal(new DateTime(2009, 1, 1), info.DateTaken);
            Assert.Equal(DateGranularity.Circa, info.DateTakenGranularity);

        }

        [Fact]
        public void PhotosGetInfoVideoTest()
        {
            const string videoId = "2926486605";

            var info = Instance.PhotosGetInfo(videoId);

            Assert.NotNull(info);
            Assert.Equal(videoId, info.PhotoId);
        }

        [Fact]
        public void TestPhotoNotFound()
        {
            Should.Throw< PhotoNotFoundException>(() => Instance.PhotosGetInfo("abcd"));
        }

        [Fact]
        public async Task TestPhotoNotFoundAsync()
        {
            var w = new AsyncSubject<FlickrResult<PhotoInfo>>();

            var r = await Instance.PhotosGetInfoAsync("abcd");
             { w.OnNext(r); w.OnCompleted(); };
            var result = w.Next().First();

            result.HasError.ShouldBeTrue();
            result.Error.ShouldBeOfType<PhotoNotFoundException>();
        }

        [Fact]
        public void ShouldReturnPhotoInfoWithGeoData()
        {
            var info = Instance.PhotosGetInfo("54071193");

            Assert.NotNull(info);//, "PhotoInfo should not be null."
            Assert.NotNull(info.Location);//, "Location should not be null."
            Assert.Equal(-180, info.Location.Longitude);//, "Longitude should be -180"
            Assert.Equal("https://www.flickr.com/photos/afdn/54071193/", info.Urls[0].Url);
            Assert.True(info.GeoPermissions.IsPublic, "GeoPermissions should be public.");
        }

        [Fact]
        public void ShouldReturnPhotoInfoWithValidUrls()
        {
            var info = Instance.PhotosGetInfo("9671143400");

            Assert.True(UrlHelper.Exists(info.Small320Url), "Small320Url is not valid url : " + info.Small320Url);
            Assert.True(UrlHelper.Exists(info.Medium640Url), "Medium640Url is not valid url : " + info.Medium640Url);
            Assert.True(UrlHelper.Exists(info.Medium800Url), "Medium800Url is not valid url : " + info.Medium800Url);
            Assert.NotEqual(info.SmallUrl, info.LargeUrl);//, "URLs should all be different."
        }

        [Fact]
        public void PhotoInfoUrlsShouldMatchSizes()
        {
            var photos =
                Instance.PhotosSearch(new PhotoSearchOptions
                                          {
                                              UserId = TestData.TestUserId,
                                              PerPage = 1,
                                              Extras = PhotoSearchExtras.AllUrls
                                          });

            var photo = photos.First();

            var info = Instance.PhotosGetInfo(photo.PhotoId);

            Assert.Equal(photo.LargeUrl, info.LargeUrl);
            Assert.Equal(photo.Small320Url, info.Small320Url);
        }

        [Theory]
        [InlineData("46611802@N00", "")]
        [InlineData("51266254@N00", "Curitiba, Brazil")]
        public void GetInfoWithInvalidXmlTests(string userId, string location)
        {
            var userInfo = Instance.PeopleGetInfo(userId);
            Assert.Equal(userInfo.UserId, (userId));
            Assert.Equal(userInfo.Location, (location));
        }

    }
}
