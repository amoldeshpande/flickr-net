using System;

using Xunit;
using FlickrNet;
using Shouldly;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for PeopleTests
    /// </summary>
    
    public class PeopleTests : BaseTest
    {
        [Fact]
        public void PeopleGetPhotosOfBasicTest()
        {
            PeoplePhotoCollection p = Instance.PeopleGetPhotosOf(TestData.TestUserId);

            Assert.NotNull(p);//, "PeoplePhotos should not be null."
            Assert.NotEqual(0, p.Count);//, "PeoplePhotos.Count should be greater than zero."
            Assert.True(p.PerPage >= p.Count, "PerPage should be the same or greater than the number of photos returned.");
        }

        [Fact]
        public void PeopleGetPhotosOfAuthRequired()
        {
            Should.Throw<SignatureRequiredException>(() => Instance.PeopleGetPhotosOf());
        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public void PeopleGetPhotosOfMe()
        {
            PeoplePhotoCollection p = AuthInstance.PeopleGetPhotosOf();

            Assert.NotNull(p);//, "PeoplePhotos should not be null."
            Assert.NotEqual(0, p.Count);//, "PeoplePhotos.Count should be greater than zero."
            Assert.True(p.PerPage >= p.Count, "PerPage should be the same or greater than the number of photos returned.");
        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public void PeopleGetPhotosBasicTest()
        {
            PhotoCollection photos = AuthInstance.PeopleGetPhotos();

            Assert.NotNull(photos);
            Assert.NotEqual(0, photos.Count);//, "Count should not be zero."
            Assert.True(photos.Total > 1000, "Total should be greater than 1000.");
        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public void PeopleGetPhotosFullParamTest()
        {
            PhotoCollection photos = AuthInstance.PeopleGetPhotos(TestData.TestUserId, SafetyLevel.Safe, new DateTime(2010, 1, 1),
                                                       new DateTime(2012, 1, 1), new DateTime(2010, 1, 1),
                                                       new DateTime(2012, 1, 1), ContentTypeSearch.All,
                                                       PrivacyFilter.PublicPhotos, PhotoSearchExtras.All, 1, 20);

            Assert.NotNull(photos);
            Assert.Equal(20, photos.Count);//, "Count should be twenty."
        }

        [Fact]
        public void PeopleGetInfoBasicTestUnauth()
        {
            Flickr f = Instance;
            Person p = f.PeopleGetInfo(TestData.TestUserId);

            Assert.Equal("Sam Judson", p.UserName);
            Assert.Equal("Sam Judson", p.RealName);
            Assert.Equal("samjudson", p.PathAlias);
            Assert.True(p.IsPro);// "IsPro should be true.");
            Assert.Equal("Newcastle, UK", p.Location);
            Assert.Equal("+00:00", p.TimeZoneOffset);
            Assert.Equal("GMT: Dublin, Edinburgh, Lisbon, London", p.TimeZoneLabel);
            Assert.NotNull(p.Description);// "Description should not be null."
            Assert.True(p.Description.Length > 0);// "Description should not be empty");
        }

        [Fact]
        public void PeopleGetInfoGenderNoAuthTest()
        {
            Flickr f = Instance;
            Person p = f.PeopleGetInfo("10973297@N00");

            Assert.NotNull(p);//, "Person object should be returned"
            Assert.Null(p.Gender);// "Gender should be null as not authenticated.");

            Assert.Null(p.IsReverseContact);// "IsReverseContact should not be null.");
            Assert.Null(p.IsContact);// "IsContact should be null.");
            Assert.Null(p.IsIgnored);// "IsIgnored should be null.");
            Assert.Null(p.IsFriend);// "IsFriend should be null.");
        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public void PeopleGetInfoGenderTest()
        {
            Flickr f = AuthInstance;
            Person p = f.PeopleGetInfo("10973297@N00");

            Assert.NotNull(p);//, "Person object should be returned"
            Assert.Equal("F", p.Gender);//, "Gender of M should be returned"

            Assert.NotNull(p.IsReverseContact);//, "IsReverseContact should not be null."
            Assert.NotNull(p.IsContact);//, "IsContact should not be null."
            Assert.NotNull(p.IsIgnored);//, "IsIgnored should not be null."
            Assert.NotNull(p.IsFriend);//, "IsFriend should not be null."

            Assert.NotNull(p.PhotosSummary);//, "PhotosSummary should not be null."
        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public void PeopleGetInfoBuddyIconTest()
        {
            Flickr f = AuthInstance;
            Person p = f.PeopleGetInfo(TestData.TestUserId);
            Assert.True(p.BuddyIconUrl.Contains(".staticflickr.com/"), "Buddy icon doesn't contain correct details.");
        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public void PeopleGetInfoSelfTest()
        {
            Flickr f = AuthInstance;

            Person p = f.PeopleGetInfo(TestData.TestUserId);

            Assert.NotNull(p.MailboxSha1Hash);//, "MailboxSha1Hash should not be null."
            Assert.NotNull(p.PhotosSummary);//, "PhotosSummary should not be null."
            Assert.NotEqual(0, p.PhotosSummary.Views);//, "PhotosSummary.Views should not be zero."

        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public void PeopleGetGroupsTest()
        {
            Flickr f = AuthInstance;

            var groups = f.PeopleGetGroups(TestData.TestUserId);

            Assert.NotNull(groups);
            Assert.NotEqual(0, groups.Count);
        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public void PeopleGetLimitsTest()
        {
            var f = AuthInstance;

            var limits = f.PeopleGetLimits();

            Assert.NotNull(limits);

            Assert.Equal(0, limits.MaximumDisplayPixels);
            Assert.Equal(209715200, limits.MaximumPhotoUpload);
            Assert.Equal(1073741824, limits.MaximumVideoUpload);
            Assert.Equal(180, limits.MaximumVideoDuration);
            
        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public void PeopleFindByUsername()
        {
            Flickr f = AuthInstance;

            FoundUser user = f.PeopleFindByUserName("Sam Judson");

            Assert.Equal("41888973@N00", user.UserId);
            Assert.Equal("Sam Judson", user.UserName);
        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public void PeopleFindByEmail()
        {
            Flickr f = AuthInstance;

            FoundUser user = f.PeopleFindByEmail("samjudson@gmail.com");

            Assert.Equal("41888973@N00", user.UserId);
            Assert.Equal("Sam Judson", user.UserName);
        }

        [Fact]
        public void PeopleGetPublicPhotosBasicTest()
        {
            var f = Instance;
            var photos = f.PeopleGetPublicPhotos(TestData.TestUserId, 1, 100, SafetyLevel.None, PhotoSearchExtras.OriginalDimensions);

            Assert.NotNull(photos);
            Assert.NotEqual(0, photos.Count);

            foreach (var photo in photos)
            {
                Assert.NotNull(photo.PhotoId);//, "PhotoId should not be null."
                Assert.Equal(TestData.TestUserId, photo.UserId);
                Assert.NotEqual(0, photo.OriginalWidth);//, "OriginalWidth should not be zero."
                Assert.NotEqual(0, photo.OriginalHeight);//, "OriginalHeight should not be zero."
            }
        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public void PeopleGetPublicGroupsBasicTest()
        {
            Flickr f = AuthInstance;

            GroupInfoCollection groups = f.PeopleGetPublicGroups(TestData.TestUserId);

            Assert.NotEqual(0, groups.Count);//, "PublicGroupInfoCollection.Count should not be zero."

            foreach(GroupInfo group in groups)
            {
                Assert.NotNull(group.GroupId);//, "GroupId should not be null."
                Assert.NotNull(group.GroupName);//, "GroupName should not be null."
            }
        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public void PeopleGetUploadStatusBasicTest()
        {
            var u = AuthInstance.PeopleGetUploadStatus();

            Assert.NotNull(u);
            Assert.NotNull(u.UserId);
            Assert.NotNull(u.UserName);
            Assert.NotEqual(0, u.FileSizeMax);
        }

        [Fact]
        public void PeopleGetInfoBlankDate()
        {
            var p = Instance.PeopleGetInfo("18387778@N00");
        }

        [Fact]
        public void PeopleGetInfoZeroDate()
        {
            var p = Instance.PeopleGetInfo("47963952@N03");
        }

        [Fact]
        public void PeopleGetInfoInternationalCharacters()
        {
            var p = Instance.PeopleGetInfo("24754141@N08");

            Assert.Equal("24754141@N08", p.UserId);//, "UserId should match."
            Assert.Equal("Pierre Hsiu 脩丕政", p.RealName);//, "RealName should match"
        }
    }
}
