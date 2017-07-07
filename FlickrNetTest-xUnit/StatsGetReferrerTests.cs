using System;

using Xunit;
using FlickrNet;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for StatsGetReferrerTests
    /// </summary>
    [Trait("Category","AccessTokenRequired")]
    public class StatsGetReferrerTests : BaseTest
    {
        string collectionId = "78188-72157600072356354";
        string photoId = "5890800";
        string photosetId = "1493109";
        readonly DateTime lastWeek = DateTime.Today.AddDays(-7);


        [Fact]
        public void StatsGetPhotoReferrersBasicTest()
        {
            string domain = "flickr.com";

            Flickr f = AuthInstance;

            StatReferrerCollection referrers = f.StatsGetPhotoReferrers(lastWeek, domain, 1, 10);

            Assert.NotNull(referrers);// "StatReferrers should not be null.");

            Assert.NotEqual(0, referrers.Total);// "StatReferrers.Total should not be zero."

            Assert.Equal(referrers.Count, Math.Min(referrers.Total, referrers.PerPage));// "Count should either be equal to Total or PerPage."

            Assert.Equal(domain, referrers.DomainName);// "StatReferrers.Domain should be the same as the searched for domain."

            foreach (StatReferrer referrer in referrers)
            {
                Assert.NotNull(referrer.Url);// "StatReferrer.Url should not be null.");
                Assert.NotEqual(0, referrer.Views);// "StatReferrer.Views should be greater than zero."
            }

            // Overloads
            referrers = f.StatsGetPhotoReferrers(lastWeek, domain);
            Assert.NotNull(referrers);

            referrers = f.StatsGetPhotoReferrers(lastWeek, domain, photoId);
            Assert.NotNull(referrers);

            referrers = f.StatsGetPhotoReferrers(lastWeek, domain, photoId, 1, 10);
            Assert.NotNull(referrers);

        }

        [Fact]
        public void StatsGetPhotosetsReferrersBasicTest()
        {
            string domain = "flickr.com";

            Flickr f = AuthInstance;

            StatReferrerCollection referrers = f.StatsGetPhotosetReferrers(lastWeek, domain, 1, 10);

            Assert.NotNull(referrers);// "StatReferrers should not be null.");

            // I often get 0 referrers for a particular given date. As this method only works for the previous 28 days I cannot pick a fixed date.
            // Therefore we cannot confirm that regerrers.Total is always greater than zero.

            Assert.Equal(referrers.Count, Math.Min(referrers.Total, referrers.PerPage));// "Count should either be equal to Total or PerPage."

            if (referrers.Total == 0) return;

            Assert.Equal(domain, referrers.DomainName);// "StatReferrers.Domain should be the same as the searched for domain."

            foreach (StatReferrer referrer in referrers)
            {
                Assert.NotNull(referrer.Url);// "StatReferrer.Url should not be null.");
                Assert.NotEqual(0, referrer.Views);// "StatReferrer.Views should be greater than zero."
            }

            // Overloads
            referrers = f.StatsGetPhotosetReferrers(lastWeek, domain);
            Assert.NotNull(referrers);

            referrers = f.StatsGetPhotosetReferrers(lastWeek, domain, photosetId);
            Assert.NotNull(referrers);

            referrers = f.StatsGetPhotosetReferrers(lastWeek, domain, photosetId, 1, 10);
            Assert.NotNull(referrers);

        }

        [Fact]
        public void StatsGetPhotostreamReferrersBasicTest()
        {
            string domain = "flickr.com";

            Flickr f = AuthInstance;

            StatReferrerCollection referrers = f.StatsGetPhotostreamReferrers(lastWeek, domain, 1, 10);

            Assert.NotNull(referrers);// "StatReferrers should not be null.");

            // I often get 0 referrers for a particular given date. As this method only works for the previous 28 days I cannot pick a fixed date.
            // Therefore we cannot confirm that regerrers.Total is always greater than zero.

            Assert.Equal(referrers.Count, Math.Min(referrers.Total, referrers.PerPage));// "Count should either be equal to Total or PerPage."

            if (referrers.Total == 0) return;

            Assert.Equal(domain, referrers.DomainName);// "StatReferrers.Domain should be the same as the searched for domain."

            foreach (StatReferrer referrer in referrers)
            {
                Assert.NotNull(referrer.Url);// "StatReferrer.Url should not be null.");
                Assert.NotEqual(0, referrer.Views);// "StatReferrer.Views should be greater than zero."
            }

            // Overloads
            referrers = f.StatsGetPhotostreamReferrers(lastWeek, domain);
            Assert.NotNull(referrers);
        }

        [Fact]
        public void StatsGetCollectionReferrersBasicTest()
        {
            string domain = "flickr.com";

            Flickr f = AuthInstance;

            var referrers = f.StatsGetCollectionReferrers(lastWeek, domain, 1, 10);

            Assert.NotNull(referrers);// "StatReferrers should not be null.");

            Assert.Equal(referrers.Count, Math.Min(referrers.Total, referrers.PerPage));// "Count should either be equal to Total or PerPage."

            if (referrers.Total == 0 && referrers.Pages == 0) return;

            Assert.Equal(domain, referrers.DomainName);// "StatReferrers.Domain should be the same as the searched for domain."

            foreach (StatReferrer referrer in referrers)
            {
                Assert.NotNull(referrer.Url);// "StatReferrer.Url should not be null.");
                Assert.NotEqual(0, referrer.Views);// "StatReferrer.Views should be greater than zero."
            }
            
            // Overloads
            referrers = f.StatsGetCollectionReferrers(lastWeek, domain);
            Assert.NotNull(referrers);

            referrers = f.StatsGetCollectionReferrers(lastWeek, domain, collectionId);
            Assert.NotNull(referrers);

            referrers = f.StatsGetCollectionReferrers(lastWeek, domain, collectionId, 1, 10);
            Assert.NotNull(referrers);
        }

    }
}
