using System;

using Xunit;
using FlickrNet;
using System.IO;
using Shouldly;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for GroupsPoolsGetGroupsTests
    /// </summary>
    
    public class GroupsPoolsTests : BaseTest
    {
       
        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public void GroupsPoolsAddBasicTest()
        {
            Flickr f = AuthInstance;

            byte[] imageBytes = TestData.TestImageBytes;
            var s = new MemoryStream(imageBytes);
            s.Position = 0;

            string title = "Test Title";
            string desc = "Test Description\nSecond Line";
            string tags = "testtag1,testtag2";
            string photoId = f.UploadPicture(s, "Test.jpg", title, desc, tags, false, false, false, ContentType.Other, SafetyLevel.Safe, HiddenFromSearch.Visible);

            try
            {
                f.GroupsPoolsAdd(photoId, TestData.FlickrNetTestGroupId);
            }
            finally
            {
                f.PhotosDelete(photoId);
            }

        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public void GroupsPoolsAddNotAuthTestTest()
        {
            string photoId = "12345";

            Should.Throw<SignatureRequiredException>(() => Instance.GroupsPoolsAdd(photoId, TestData.FlickrNetTestGroupId));
        }

        [Fact]
        public void GroupsPoolGetPhotosFullParamTest()
        {
            Flickr f = Instance;

            PhotoCollection photos = f.GroupsPoolsGetPhotos(TestData.GroupId, null, TestData.TestUserId, PhotoSearchExtras.All, 1, 20);

            Assert.NotNull(photos);//, "Photos should not be null"
            Assert.True(photos.Count > 0, "Should be more than 0 photos returned");
            Assert.Equal(20, photos.PerPage);
            Assert.Equal(1, photos.Page);

            foreach (Photo p in photos)
            {
                Assert.NotEqual(default(DateTime), p.DateAddedToGroup);//, "DateAddedToGroup should not be default value"
                Assert.True(p.DateAddedToGroup < DateTime.Now, "DateAddedToGroup should be in the past");
            }

        }

        [Fact]
        public void GroupsPoolGetPhotosDateAddedTest()
        {
            Flickr f = Instance;

            PhotoCollection photos = f.GroupsPoolsGetPhotos(TestData.GroupId);

            Assert.NotNull(photos);//, "Photos should not be null"
            Assert.True(photos.Count > 0, "Should be more than 0 photos returned");

            foreach (Photo p in photos)
            {
                Assert.NotEqual(default(DateTime), p.DateAddedToGroup);//, "DateAddedToGroup should not be default value"
                Assert.True(p.DateAddedToGroup < DateTime.Now, "DateAddedToGroup should be in the past");
            }

        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public void GroupsPoolsGetGroupsBasicTest()
        {
            MemberGroupInfoCollection groups = AuthInstance.GroupsPoolsGetGroups();

            Assert.NotNull(groups);//, "MemberGroupInfoCollection should not be null."
            Assert.NotEqual(0, groups.Count);//, "MemberGroupInfoCollection.Count should not be zero."
            Assert.True(groups.Count > 1, "Count should be greater than one.");

            Assert.Equal(400, groups.PerPage);//, "PerPage should be 400."
            Assert.Equal(1, groups.Page);//, "Page should be 1."
            Assert.True(groups.Total > 1, "Total chould be greater than one");
        }
    }
}
