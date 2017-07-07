using System;

using Xunit;
using FlickrNet;

namespace FlickrNetTest
{
    [Trait("Category","AccessTokenRequired")]
    public class StatsGetDomainsTests : BaseTest
    {
        string collectionId = "78188-72157600072356354";
        string photoId = "5890800";
        string photosetId = "1493109";

        [Fact]
        public void StatsGetCollectionDomainsBasicTest()
        {
            Flickr f = AuthInstance;

            var domains = f.StatsGetCollectionDomains(DateTime.Today.AddDays(-2));

            Assert.NotNull(domains);//, "StatDomains should not be null."
            Assert.Equal(domains.Total, domains.Count);//, "StatDomains.Count should be the same as StatDomains.Total"

            // Overloads
            domains = f.StatsGetCollectionDomains(DateTime.Today.AddDays(-2), collectionId);
            Assert.NotNull(domains);

            domains = f.StatsGetCollectionDomains(DateTime.Today.AddDays(-2), 1, 10);
            Assert.NotNull(domains);

            domains = f.StatsGetCollectionDomains(DateTime.Today.AddDays(-2), collectionId, 1, 10);
            Assert.NotNull(domains);
        }

        [Fact]
        public void StatsGetCollectionStatsTest()
        {
            Flickr f = AuthInstance;

            Stats stats = f.StatsGetCollectionStats(DateTime.Today.AddDays(-2), collectionId);

            Assert.NotNull(stats);//, "Stats should not be null."
        }

        [Fact]
        public void StatsGetPhotoDomainsTests()
        {
            Flickr f = AuthInstance;

            var domains = f.StatsGetPhotoDomains(DateTime.Today.AddDays(-2));
            Assert.NotNull(domains);//, "StatDomains should not be null."
            Assert.NotEqual(0, domains.Count);//, "StatDomains.Count should not be zero."

            foreach (StatDomain domain in domains)
            {
                Assert.NotNull(domain.Name);//, "StatDomain.Name should not be null."
                Assert.NotEqual(0, domain.Views);//, "StatDomain.Views should not be zero."
            }

            // Overloads
            domains = f.StatsGetPhotoDomains(DateTime.Today.AddDays(-2), photoId);
            Assert.NotNull(domains);//, "StatDomains should not be null."

            domains = f.StatsGetPhotoDomains(DateTime.Today.AddDays(-2), photoId, 1, 10);
            Assert.NotNull(domains);//, "StatDomains should not be null."

            domains = f.StatsGetPhotoDomains(DateTime.Today.AddDays(-2), 1, 10);
            Assert.NotNull(domains);//, "StatDomains should not be null."

        }

        [Fact]
        public void StatsGetPhotoStatsTest()
        {
            Flickr f = AuthInstance;

            var stats = f.StatsGetPhotoStats(DateTime.Today.AddDays(-5), photoId);

            Assert.NotNull(stats);
        }

        [Fact]
        public void StatsGetPhotosetDomainsBasic()
        {
            Flickr f = AuthInstance;

            var domains = f.StatsGetPhotosetDomains(DateTime.Today.AddDays(-2));
            Assert.NotNull(domains);//, "StatDomains should not be null."

            foreach (StatDomain domain in domains)
            {
                Assert.NotNull(domain.Name);//, "StatDomain.Name should not be null."
                Assert.NotEqual(0, domain.Views);//, "StatDomain.Views should not be zero."
            }

            // Overloads
            domains = f.StatsGetPhotosetDomains(DateTime.Today.AddDays(-2), 1, 10);
            Assert.NotNull(domains);//, "StatDomains should not be null."

            domains = f.StatsGetPhotosetDomains(DateTime.Today.AddDays(-2), photosetId);
            Assert.NotNull(domains);//, "StatDomains should not be null."

            domains = f.StatsGetPhotosetDomains(DateTime.Today.AddDays(-2), photosetId, 1, 10);
            Assert.NotNull(domains);//, "StatDomains should not be null."


        }

        [Fact]
        public void StatsGetPhotosetStatsTest()
        {
            Flickr f = AuthInstance;

            var stats = f.StatsGetPhotosetStats(DateTime.Today.AddDays(-5), photosetId);

            Assert.NotNull(stats);
        }

        [Fact]
        public void StatsGetPhotostreamDomainsBasic()
        {
            Flickr f = AuthInstance;

            var domains = f.StatsGetPhotostreamDomains(DateTime.Today.AddDays(-2));
            Assert.NotNull(domains);//, "StatDomains should not be null."

            foreach (StatDomain domain in domains)
            {
                Assert.NotNull(domain.Name);//, "StatDomain.Name should not be null."
                Assert.NotEqual(0, domain.Views);//, "StatDomain.Views should not be zero."
            }

            // Overload
            domains = f.StatsGetPhotostreamDomains(DateTime.Today.AddDays(-2), 1, 10);
            Assert.NotNull(domains);//, "StatDomains should not be null."
        }

        [Fact]
        public void StatsGetPhotostreamStatsTest()
        {
            Flickr f = AuthInstance;

            var stats = f.StatsGetPhotostreamStats(DateTime.Today.AddDays(-5));

            Assert.NotNull(stats);
        }


    }
}
