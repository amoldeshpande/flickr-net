
using Xunit;
using FlickrNet;
using Shouldly;

namespace FlickrNetTest
{
    
    public class TagsTests : BaseTest
    {
        public TagsTests()
        {
            Flickr.CacheDisabled = true;
        }

        [Fact]
        public void TagsGetListUserRawAuthenticationTest()
        {
            Flickr f = Instance;
            Should.Throw<SignatureRequiredException>(() => f.TagsGetListUserRaw());
        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]

        public void TagsGetListUserRawBasicTest()
        {
            var tags = AuthInstance.TagsGetListUserRaw();

            Assert.NotEqual(0, tags.Count);//, "There should be one or more raw tags returned"

            foreach (RawTag tag in tags)
            {
                Assert.NotNull(tag.CleanTag);//, "Clean tag should not be null"
                Assert.True(tag.CleanTag.Length > 0, "Clean tag should not be empty string");
                Assert.True(tag.RawTags.Count > 0, "Should be one or more raw tag for each clean tag");
            }
        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]

        public void TagsGetListUserPopularBasicTest()
        {
            TagCollection tags = AuthInstance.TagsGetListUserPopular();

            Assert.NotNull(tags);//, "TagCollection should not be null."
            Assert.NotEqual(0, tags.Count);//, "TagCollection.Count should not be zero."

            foreach (Tag tag in tags)
            {
                Assert.NotNull(tag.TagName);//, "Tag.TagName should not be null."
                Assert.NotEqual(0, tag.Count);//, "Tag.Count should not be zero."
            }
        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]

        public void TagsGetListUserBasicTest()
        {
            TagCollection tags = AuthInstance.TagsGetListUser();

            Assert.NotNull(tags);//, "TagCollection should not be null."
            Assert.NotEqual(0, tags.Count);//, "TagCollection.Count should not be zero."

            foreach (Tag tag in tags)
            {
                Assert.NotNull(tag.TagName);//, "Tag.TagName should not be null."
                Assert.Equal(0, tag.Count);//, "Tag.Count should be zero. Not ser for this method."
            }
        }

        [Fact]
        public void TagsGetListPhotoBasicTest()
        {
            var tags = Instance.TagsGetListPhoto(TestData.PhotoId);

            Assert.NotNull(tags);//, "tags should not be null."
            Assert.NotEqual(0, tags.Count);//, "Length should be greater than zero."

            foreach (var tag in tags)
            {
                Assert.NotNull(tag.TagId);//, "TagId should not be null."
                Assert.NotNull(tag.TagText);//, "TagText should not be null."
                Assert.NotNull(tag.Raw);//, "Raw should not be null."
                Assert.NotNull(tag.IsMachineTag);//, "IsMachineTag should not be null."
            }

        }

        [Fact]
        public void TagsGetClustersNewcastleTest()
        {
            var col = Instance.TagsGetClusters("newcastle");

            Assert.NotNull(col);

            Assert.Equal(4, col.Count);//, "Count should be four."
            Assert.Equal(col.TotalClusters, col.Count);
            Assert.Equal("newcastle", col.SourceTag);

            Assert.Equal("water-ocean-clouds", col[0].ClusterId);

            foreach (var c in col)
            {
                Assert.NotEqual(0, c.TotalTags);//, "TotalTags should not be zero."
                Assert.NotNull(c.Tags);//, "Tags should not be null."
                Assert.True(c.Tags.Count >= 3);
                Assert.NotNull(c.ClusterId);
            }
        }

        [Fact]
        public void TagsGetClusterPhotosNewcastleTest()
        {
            Flickr f = Instance;
            var col = f.TagsGetClusters("newcastle");

            foreach (var c in col)
            {
                var ps = f.TagsGetClusterPhotos(c);
                Assert.NotNull(ps);
                Assert.NotEqual(0, ps.Count);
            }
        }

        [Fact]
        public void TagsGetHotListTest()
        {
            var col = Instance.TagsGetHotList();

            Assert.NotEqual(0, col.Count);//, "Count should not be zero."

            foreach (var c in col)
            {
                Assert.NotNull(c);
                Assert.NotNull(c.Tag);
                Assert.NotEqual(0, c.Score);
            }
        }

        [Fact]
        public void TagsGetListUserTest()
        {
            var col = Instance.TagsGetListUser(TestData.TestUserId);
        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public void TagsGetMostFrequentlyUsedTest()
        {
            Flickr f = AuthInstance;

            var tags = f.TagsGetMostFrequentlyUsed();

            Assert.NotNull(tags);

            Assert.NotEqual(0, tags.Count);
        }
    }
}
