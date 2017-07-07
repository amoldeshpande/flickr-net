
using Xunit;
using FlickrNet;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for CollectionGetTreeTest
    /// </summary>
    
    public class CollectionTests : BaseTest
    {
        
        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public void CollectionGetInfoBasicTest()
        {
            string id = "78188-72157618817175751";

            Flickr f = AuthInstance;

            CollectionInfo info = f.CollectionsGetInfo(id);

            Assert.Equal(id, info.CollectionId);//, "CollectionId should be correct."
            Assert.Equal(1, info.ChildCount);//, "ChildCount should be 1."
            Assert.Equal("Global Collection", info.Title);//, "Title should be 'Global Collection'."
            Assert.Equal("My global collection.", info.Description);//, "Description should be set correctly."
            Assert.Equal("3629", info.Server);//, "Server should be 3629."

            Assert.Equal(12, info.IconPhotos.Count);//, "IconPhotos.Length should be 12."

            Assert.Equal("Tires", info.IconPhotos[0].Title);//, "The first IconPhoto Title should be 'Tires'."
        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public void CollectionGetTreeRootTest()
        {
            Flickr f = AuthInstance;
            CollectionCollection tree = f.CollectionsGetTree();

            Assert.NotNull(tree);//, "CollectionList should not be null."
            Assert.NotEqual(0, tree.Count);//, "CollectionList.Count should not be zero."

            foreach (Collection coll in tree)
            {
                Assert.NotNull(coll.CollectionId);//, "CollectionId should not be null."
                Assert.NotNull(coll.Title);//, "Title should not be null."
                Assert.NotNull(coll.Description);//, "Description should not be null."
                Assert.NotNull(coll.IconSmall);//, "IconSmall should not be null."
                Assert.NotNull(coll.IconLarge);//, "IconLarge should not be null."

                Assert.NotEqual(0, coll.Sets.Count + coll.Collections.Count);//, "Should be either some sets or some collections."

                foreach (CollectionSet set in coll.Sets)
                {
                    Assert.NotNull(set.SetId);//, "SetId should not be null."
                }
            }
        }

        [Fact]
        public void CollectionGetTreeRootForSpecificUser()
        {
            Flickr f = Instance;
            CollectionCollection tree = f.CollectionsGetTree(null, TestData.TestUserId);

            Assert.NotNull(tree);//, "CollectionList should not be null."
            Assert.NotEqual(0, tree.Count);//, "CollectionList.Count should not be zero."

            foreach (Collection coll in tree)
            {
                Assert.NotNull(coll.CollectionId);//, "CollectionId should not be null."
                Assert.NotNull(coll.Title);//, "Title should not be null."
                Assert.NotNull(coll.Description);//, "Description should not be null."
                Assert.NotNull(coll.IconSmall);//, "IconSmall should not be null."
                Assert.NotNull(coll.IconLarge);//, "IconLarge should not be null."

                Assert.NotEqual(0, coll.Sets.Count + coll.Collections.Count);//, "Should be either some sets or some collections."

                foreach (CollectionSet set in coll.Sets)
                {
                    Assert.NotNull(set.SetId);//, "SetId should not be null."
                }
            }
        }

        [Fact]
        public void CollectionGetSubTreeForSpecificUser()
        {
            string id = "78188-72157618817175751";
            Flickr f = Instance;
            CollectionCollection tree = f.CollectionsGetTree(id, TestData.TestUserId);

            Assert.NotNull(tree);//, "CollectionList should not be null."
            Assert.NotEqual(0, tree.Count);//, "CollectionList.Count should not be zero."

            foreach (Collection coll in tree)
            {
                Assert.NotNull(coll.CollectionId);//, "CollectionId should not be null."
                Assert.NotNull(coll.Title);//, "Title should not be null."
                Assert.NotNull(coll.Description);//, "Description should not be null."
                Assert.NotNull(coll.IconSmall);//, "IconSmall should not be null."
                Assert.NotNull(coll.IconLarge);//, "IconLarge should not be null."

                Assert.NotEqual(0, coll.Sets.Count + coll.Collections.Count);//, "Should be either some sets or some collections."

                foreach (CollectionSet set in coll.Sets)
                {
                    Assert.NotNull(set.SetId);//, "SetId should not be null."
                }
            }
        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public void CollectionsEditMetaTest()
        {
            string id = "78188-72157618817175751";

            Flickr.CacheDisabled = true;
            Flickr f = AuthInstance;

            CollectionInfo info = f.CollectionsGetInfo(id);

            f.CollectionsEditMeta(id, info.Title, info.Description + "TEST");

            var info2 = f.CollectionsGetInfo(id);

            Assert.NotEqual(info.Description, info2.Description);

            // Revert description
            f.CollectionsEditMeta(id, info.Title, info.Description);

        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]
        public void CollectionsEmptyCollection()
        {
            Flickr f = AuthInstance;

            // Get global collection
            CollectionCollection collections = f.CollectionsGetTree("78188-72157618817175751", null);

            Assert.NotNull(collections);
            Assert.True(collections.Count > 0, "Global collection should be greater than zero.");

            var col = collections[0];

            Assert.Equal("Global Collection", col.Title);//, "Global Collection title should be correct."

            Assert.NotNull(col.Collections);//, "Child collections property should not be null."
            Assert.True(col.Collections.Count > 0, "Global collection should have child collections.");

            var subsol = col.Collections[0];

            Assert.NotNull(subsol.Collections);//, "Child collection Collections property should ne null."
            Assert.Equal(0, subsol.Collections.Count);//, "Child collection should not have and sub collections."

        }
    }
}
