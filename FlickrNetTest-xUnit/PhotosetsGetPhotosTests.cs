using System;
using System.Linq;
using Xunit;
using FlickrNet;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for FlickrPhotoSetGetPhotos
    /// </summary>
    
    public class PhotosetsGetPhotosTests : BaseTest
    {
        [Fact]
        public void PhotosetsGetPhotosBasicTest()
        {
            PhotosetPhotoCollection set = Instance.PhotosetsGetPhotos("72157618515066456", PhotoSearchExtras.All, PrivacyFilter.None, 1, 10);

            Assert.Equal(8, set.Total);//, "NumberOfPhotos should be 8."
            Assert.Equal(8, set.Count);//, "Should be 8 photos returned."
        }

        [Fact]
        public void PhotosetsGetPhotosMachineTagsTest()
        {
            var set = Instance.PhotosetsGetPhotos("72157594218885767", PhotoSearchExtras.MachineTags, PrivacyFilter.None, 1, 10);

            var machineTagsFound = set.Any(p => !string.IsNullOrEmpty(p.MachineTags));

            Assert.True(machineTagsFound, "No machine tags were found in the photoset");
        }

        [Fact]
        public void PhotosetsGetPhotosFilterMediaTest()
        {
            // https://www.flickr.com/photos/sgoralnick/sets/72157600283870192/
            // Set contains videos and photos
            var theset = Instance.PhotosetsGetPhotos("72157600283870192", PhotoSearchExtras.Media, PrivacyFilter.None, 1, 100, MediaType.Videos);

            Assert.Equal("Canon 5D", theset.Title);

            foreach (var p in theset)
            {
                Assert.Equal("video", p.Media);//, "Should be video."
            }

            var theset2 = Instance.PhotosetsGetPhotos("72157600283870192", PhotoSearchExtras.Media, PrivacyFilter.None, 1, 100, MediaType.Photos);
            foreach (var p in theset2)
            {
                Assert.Equal("photo", p.Media);//, "Should be photo."
            }

        }

        [Fact]
        public void PhotosetsGetPhotosWebUrlTest()
        {
            var theset = Instance.PhotosetsGetPhotos("72157618515066456");

            foreach(var p in theset)
            {
                Assert.NotNull(p.UserId);//, "UserId should not be null."
                Assert.NotEqual(string.Empty, p.UserId);//, "UserId should not be an empty string."
                var url = "https://www.flickr.com/photos/" + p.UserId + "/" + p.PhotoId + "/";
                Assert.Equal(url, p.WebUrl);
            }
        }

        [Fact]
        public void PhotosetsGetPhotosPrimaryPhotoTest()
        {
            var theset = Instance.PhotosetsGetPhotos("72157618515066456", 1, 100);

            Assert.NotNull(theset.PrimaryPhotoId);//, "PrimaryPhotoId should not be null."

            if (theset.Total >= theset.PerPage) return;

            var primary = theset.FirstOrDefault(p => p.PhotoId == theset.PrimaryPhotoId);

            Assert.NotNull(primary);//, "Primary photo should have been found."
        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public void PhotosetsGetPhotosOrignalTest()
        {
            var photos = AuthInstance.PhotosetsGetPhotos("72157623027759445", PhotoSearchExtras.AllUrls);

            foreach (var photo in photos)
            {
                Assert.NotNull(photo.OriginalUrl);//, "Original URL should not be null."
            }
        }

        [Fact]
        public void ShouldReturnDateTakenWhenAsked()
        {
            var theset = Instance.PhotosetsGetPhotos("72157618515066456", PhotoSearchExtras.DateTaken | PhotoSearchExtras.DateUploaded, 1, 10);

            var firstInvalid = theset.FirstOrDefault(p => p.DateTaken == DateTime.MinValue || p.DateUploaded == DateTime.MinValue);

            Assert.Null(firstInvalid);// "There should not be a photo with not date taken or date uploaded");

            theset = Instance.PhotosetsGetPhotos("72157618515066456", PhotoSearchExtras.All, 1, 10);

            firstInvalid = theset.FirstOrDefault(p => p.DateTaken == DateTime.MinValue || p.DateUploaded == DateTime.MinValue);

            Assert.Null(firstInvalid);// "There should not be a photo with not date taken or date uploaded");

            theset = Instance.PhotosetsGetPhotos("72157618515066456", PhotoSearchExtras.None, 1, 10);

            var noDateCount = theset.Count(p => p.DateTaken == DateTime.MinValue || p.DateUploaded == DateTime.MinValue);

            Assert.Equal(theset.Count, noDateCount);//, "All photos should have no date taken set."
        }
    }
}
