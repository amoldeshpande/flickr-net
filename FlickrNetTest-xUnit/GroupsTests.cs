
using Xunit;
using FlickrNet;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for GroupsBrowseTests
    /// </summary>
    
    public class GroupsTests : BaseTest
    {
        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public void GroupsBrowseBasicTest()
        {
            Flickr f = AuthInstance;
            GroupCategory cat = f.GroupsBrowse();

            Assert.NotNull(cat);//, "GroupCategory should not be null."
            Assert.Equal("/", cat.CategoryName);//, "CategoryName should be '/'."
            Assert.Equal("/", cat.Path);//, "Path should be '/'."
            Assert.Equal("", cat.PathIds);//, "PathIds should be empty string."
            Assert.Equal(0, cat.Subcategories.Count);//, "No sub categories should be returned."
            Assert.Equal(0, cat.Groups.Count);//, "No groups should be returned."
        }

        [Fact]
        public void GroupsSearchBasicTest()
        {
            Flickr f = Instance;

            GroupSearchResultCollection results = f.GroupsSearch("Buses");

            Assert.NotNull(results);//, "GroupSearchResults should not be null."
            Assert.NotEqual(0, results.Count);//, "Count should not be zero."
            Assert.NotEqual(0, results.Total);//, "Total should not be zero."
            Assert.NotEqual(0, results.PerPage);//, "PerPage should not be zero."
            Assert.Equal(1, results.Page);//, "Page should be 1."

            foreach (GroupSearchResult result in results)
            {
                Assert.NotNull(result.GroupId);//, "GroupId should not be null."
                Assert.NotNull(result.GroupName);//, "GroupName should not be null."
            }
        }

        [Fact]
        public void GroupsGetInfoBasicTest()
        {
            Flickr f = Instance;

            GroupFullInfo info = f.GroupsGetInfo(TestData.GroupId);

            Assert.NotNull(info);//, "GroupFullInfo should not be null"
            Assert.Equal(TestData.GroupId, info.GroupId);
            Assert.Equal("The Lake District UK", info.GroupName);

            Assert.Equal("5128", info.IconServer);
            Assert.Equal("6", info.IconFarm);

            Assert.Equal("https://farm6.staticflickr.com/5128/buddyicons/53837206@N00.jpg", info.GroupIconUrl);

            Assert.Equal(2, info.ThrottleInfo.Count);
            Assert.Equal(GroupThrottleMode.PerDay, info.ThrottleInfo.Mode);

            Assert.True(info.Restrictions.PhotosAccepted, "PhotosAccepted should be true.");
            Assert.False(info.Restrictions.VideosAccepted, "VideosAccepted should be false.");
        }

        [Fact]
        public void GroupsGetInfoNoGroupIconTest()
        {
            string groupId = "562176@N20";
            Flickr f = Instance;

            GroupFullInfo info = f.GroupsGetInfo(groupId);

            Assert.NotNull(info);//, "GroupFullInfo should not be null"
            Assert.Equal("0", info.IconServer);//, "Icon Server should be zero"
            Assert.Equal("https://www.flickr.com/images/buddyicon.jpg", info.GroupIconUrl);

        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public void GroupsMembersGetListBasicTest()
        {
            var ms = AuthInstance.GroupsMembersGetList(TestData.GroupId);

            Assert.NotNull(ms);
            Assert.NotEqual(0, ms.Count);//, "Count should not be zero."
            Assert.NotEqual(0, ms.Total);//, "Total should not be zero."
            Assert.Equal(1, ms.Page);//, "Page should be one."
            Assert.NotEqual(0, ms.PerPage);//, "PerPage should not be zero."
            Assert.NotEqual(0, ms.Pages);//, "Pages should not be zero."

        }
    }
}
