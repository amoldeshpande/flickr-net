using System;
using System.IO;
using System.Linq;
using FlickrNet;
using Xunit;
using Shouldly;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for FlickrPhotosetsGetList
    /// </summary>
    
    public class PhotosetsTests : BaseTest
    {
        [Fact]
        public void GetContextTest()
        {
            const string photosetId = "72157594532130119";

            var photos = Instance.PhotosetsGetPhotos(photosetId);

            var firstPhoto = photos.First();
            var lastPhoto = photos.Last();

            var context1 = Instance.PhotosetsGetContext(firstPhoto.PhotoId, photosetId);

            Assert.NotNull(context1);//, "Context should not be null."
            Assert.Null(context1.PreviousPhoto);// "PreviousPhoto should be null for first photo.");
            Assert.NotNull(context1.NextPhoto);//, "NextPhoto should not be null."

            if (firstPhoto.PhotoId != lastPhoto.PhotoId)
            {
                Assert.Equal(photos[1].PhotoId, context1.NextPhoto.PhotoId);//, "NextPhoto should be the second photo in photoset."
            }

            var context2 = Instance.PhotosetsGetContext(lastPhoto.PhotoId, photosetId);

            Assert.NotNull(context2);//, "Last photo context should not be null."
            Assert.NotNull(context2.PreviousPhoto);//, "PreviousPhoto should not be null for first photo."
            Assert.Null(context2.NextPhoto);// "NextPhoto should be null.");

            if (firstPhoto.PhotoId != lastPhoto.PhotoId)
            {
                Assert.Equal(photos[photos.Count - 2].PhotoId, context2.PreviousPhoto.PhotoId);//, "PreviousPhoto should be the last but one photo in photoset."
            }
        }


        [Fact]
        public void PhotosetsGetInfoBasicTest()
        {
            const string photosetId = "72157594532130119";

            var p = Instance.PhotosetsGetInfo(photosetId);

            Assert.NotNull(p);
            Assert.Equal(photosetId, p.PhotosetId);
            Assert.Equal("Places: Derwent Walk, Gateshead", p.Title);
            Assert.Equal("It's near work, so I go quite a bit...", p.Description);
        }

        [Fact]
        public void PhotosetsGetListBasicTest()
        {
            PhotosetCollection photosets = Instance.PhotosetsGetList(TestData.TestUserId);

            Assert.True(photosets.Count > 0, "Should be at least one photoset");
            Assert.True(photosets.Count > 100, "Should be greater than 100 photosets. (" + photosets.Count + " returned)");

            foreach (Photoset set in photosets)
            {
                Assert.NotNull(set.OwnerId);//, "OwnerId should not be null"
                Assert.True(set.NumberOfPhotos > 0, "NumberOfPhotos should be greater than zero");
                Assert.NotNull(set.Title);//, "Title should not be null"
                Assert.NotNull(set.Description);//, "Description should not be null"
                Assert.Equal(TestData.TestUserId, set.OwnerId);
            }
        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public void PhotosetsGetListWithExtras()
        {
            var testUserPhotoSets = AuthInstance.PhotosetsGetList(TestData.TestUserId, 1, 5, PhotoSearchExtras.All);

            testUserPhotoSets.Count.ShouldBeGreaterThan(0, "Should have returned at least 1 set for the authenticated user.");

            var firstPhotoSet = testUserPhotoSets.First();

            firstPhotoSet.PrimaryPhoto.ShouldNotBeNull("Primary Photo should not be null.");
            firstPhotoSet.PrimaryPhoto.LargeSquareThumbnailUrl.ShouldNotBeNullOrEmpty("LargeSquareThumbnailUrl should not be empty.");
        }

        [Fact]
        public void PhotosetsGetListWebUrlTest()
        {
            PhotosetCollection photosets = Instance.PhotosetsGetList(TestData.TestUserId);

            Assert.True(photosets.Count > 0, "Should be at least one photoset");

            foreach (Photoset set in photosets)
            {
                Assert.NotNull(set.Url);
                string expectedUrl = "https://www.flickr.com/photos/" + TestData.TestUserId + "/sets/" + set.PhotosetId + "/";
                Assert.Equal(expectedUrl, set.Url);
            }
        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public void PhotosetsCreateAddPhotosTest()
        {
            byte[] imageBytes = TestData.TestImageBytes;
            var s = new MemoryStream(imageBytes);

            const string initialPhotoTitle = "Test Title";
            const string updatedPhotoTitle = "New Test Title";
            const string initialPhotoDescription = "Test Description\nSecond Line";
            const string updatedPhotoDescription = "New Test Description";
            const string initialTags = "testtag1,testtag2";

            s.Position = 0;
            // Upload photo once
            var photoId1 = AuthInstance.UploadPicture(s, "Test1.jpg", initialPhotoTitle, initialPhotoDescription, initialTags, false, false, false, ContentType.Other, SafetyLevel.Safe, HiddenFromSearch.Visible);

            s.Position = 0;
            // Upload photo a second time
            var photoId2 = AuthInstance.UploadPicture(s, "Test2.jpg", initialPhotoTitle, initialPhotoDescription, initialTags, false, false, false, ContentType.Other, SafetyLevel.Safe, HiddenFromSearch.Visible);

            // Creat photoset
            Photoset photoset = AuthInstance.PhotosetsCreate("Test photoset", photoId1);

            try
            {
                var photos = AuthInstance.PhotosetsGetPhotos(photoset.PhotosetId, PhotoSearchExtras.OriginalFormat | PhotoSearchExtras.Media, PrivacyFilter.None, 1, 30, MediaType.None);

                photos.Count.ShouldBe(1, "Photoset should contain 1 photo");
                photos[0].IsPublic.ShouldBe(false, "Photo 1 should be private");

                // Add second photo to photoset.
                AuthInstance.PhotosetsAddPhoto(photoset.PhotosetId, photoId2);

                // Remove second photo from photoset
                AuthInstance.PhotosetsRemovePhoto(photoset.PhotosetId, photoId2);

                AuthInstance.PhotosetsEditMeta(photoset.PhotosetId, updatedPhotoTitle, updatedPhotoDescription);

                photoset = AuthInstance.PhotosetsGetInfo(photoset.PhotosetId);

                photoset.Title.ShouldBe(updatedPhotoTitle, "New Title should be set.");
                photoset.Description.ShouldBe(updatedPhotoDescription, "New description should be set");

                AuthInstance.PhotosetsEditPhotos(photoset.PhotosetId, photoId1, new[] { photoId2, photoId1 });

                AuthInstance.PhotosetsRemovePhoto(photoset.PhotosetId, photoId2);
            }
            finally
            {
                // Delete photoset completely
                AuthInstance.PhotosetsDelete(photoset.PhotosetId);

                // Delete both photos.
                AuthInstance.PhotosDelete(photoId1);
                AuthInstance.PhotosDelete(photoId2);
            }
        }

        [Fact]
        public void PhotosetsGetInfoEncodingCorrect()
        {
            Photoset pset = Instance.PhotosetsGetInfo("72157627650627399");

            Assert.Equal("Sítio em Arujá - 14/08/2011", pset.Title);
        }
    }
}
