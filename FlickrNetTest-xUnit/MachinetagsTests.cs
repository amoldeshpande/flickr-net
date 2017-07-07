using System;

using Xunit;
using FlickrNet;

namespace FlickrNetTest
{
    
    public class MachinetagsTests : BaseTest
    {
        [Fact]
        public void MachinetagsGetNamespacesBasicTest()
        {
            NamespaceCollection col = Instance.MachineTagsGetNamespaces();

            Assert.True(col.Count > 10, "Should be greater than 10 namespaces.");

            foreach (var n in col)
            {
                Assert.NotNull(n.NamespaceName);
                Assert.NotEqual(0, n.Predicates);//, "Predicates should not be zero."
                Assert.NotEqual(0, n.Usage);//, "Usage should not be zero."
            }
        }

        [Fact]
        public void MachinetagsGetPredicatesBasicTest()
        {
            var col = Instance.MachineTagsGetPredicates();

            Assert.True(col.Count > 10, "Should be greater than 10 namespaces.");

            foreach (var n in col)
            {
                Assert.NotNull(n.PredicateName);
                Assert.NotEqual(0, n.Namespaces);//, "Namespaces should not be zero."
                Assert.NotEqual(0, n.Usage);//, "Usage should not be zero."
            }
        }

        [Fact]
        public void MachinetagsGetPairsBasicTest()
        {
            var pairs = Instance.MachineTagsGetPairs(null, null, 0, 0);
            Assert.NotNull(pairs);

            Assert.NotEqual(0, pairs.Count);//, "Count should not be zero."

            foreach (Pair p in pairs)
            {
                Assert.NotNull(p.NamespaceName);
                Assert.NotNull(p.PairName);
                Assert.NotNull(p.PredicateName);
                Assert.NotEqual(0, p.Usage);//, "Usage should be greater than zero."
            }
        }


        [Fact]
        public void MachinetagsGetPairsNamespaceTest()
        {
            var pairs = Instance.MachineTagsGetPairs("dc", null, 0, 0);
            Assert.NotNull(pairs);

            Assert.NotEqual(0, pairs.Count);//, "Count should not be zero."

            foreach (Pair p in pairs)
            {
                Assert.Equal("dc", p.NamespaceName);//, "NamespaceName should be 'dc'."
                Assert.NotNull(p.PairName);
                Assert.True(p.PairName.StartsWith("dc:", StringComparison.Ordinal), "PairName should start with 'dc:'.");
                Assert.NotNull(p.PredicateName);
                Assert.NotEqual(0, p.Usage);//, "Usage should be greater than zero."

            }
        }

        [Fact]
        public void MachinetagsGetPairsPredicateTest()
        {
            var pairs = Instance.MachineTagsGetPairs(null, "author", 0, 0);
            Assert.NotNull(pairs);

            Assert.NotEqual(0, pairs.Count);//, "Count should not be zero."

            foreach (Pair p in pairs)
            {
                Assert.Equal("author", p.PredicateName);//, "PredicateName should be 'dc'."
                Assert.NotNull(p.PairName);
                Assert.True(p.PairName.EndsWith(":author", StringComparison.Ordinal), "PairName should end with ':author'.");
                Assert.NotNull(p.NamespaceName);
                Assert.NotEqual(0, p.Usage);//, "Usage should be greater than zero."

            }
        }

        [Fact]
        public void MachinetagsGetPairsDcAuthorTest()
        {
            var pairs = Instance.MachineTagsGetPairs("dc", "author", 0, 0);
            Assert.NotNull(pairs);

            Assert.Equal(1, pairs.Count);//, "Count should be 1."

            foreach (Pair p in pairs)
            {
                Assert.Equal("author", p.PredicateName);//, "PredicateName should be 'author'."
                Assert.Equal("dc", p.NamespaceName);//, "NamespaceName should be 'dc'."
                Assert.Equal("dc:author", p.PairName);//, "PairName should be 'dc:author'."
                Assert.NotEqual(0, p.Usage);//, "Usage should be greater than zero."

            }
        }

        [Fact]
        public void MachinetagsGetValuesTest()
        {
            var items = Instance.MachineTagsGetValues("dc", "author");
            Assert.NotNull(items);

            Assert.NotEqual(0, items.Count);//, "Count should be not be zero."
            Assert.Equal("dc", items.NamespaceName);
            Assert.Equal("author", items.PredicateName);

            foreach (var item in items)
            {
                Assert.Equal("author", item.PredicateName);//, "PredicateName should be 'author'."
                Assert.Equal("dc", item.NamespaceName);//, "NamespaceName should be 'dc'."
                Assert.NotNull(item.ValueText);
                Assert.NotEqual(0, item.Usage);//, "Usage should be greater than zero."
            }
        }

        [Fact]
        public void MachinetagsGetRecentValuesTest()
        {
            var items = Instance.MachineTagsGetRecentValues(DateTime.Now.AddHours(-5));
            Assert.NotNull(items);

            Assert.NotEqual(0, items.Count);//, "Count should be not be zero."

            foreach (var item in items)
            {
                Assert.NotNull(item.NamespaceName);
                Assert.NotNull(item.PredicateName);
                Assert.NotNull(item.ValueText);
                Assert.NotNull(item.DateFirstAdded);
                Assert.NotNull(item.DateLastUsed);
                Assert.NotEqual(0, item.Usage);//, "Usage should be greater than zero."
            }
        }
    }
}
