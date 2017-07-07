using System;
using System.Collections.Generic;

using Xunit;
using FlickrNet;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for PhotosGetCountTests
    /// </summary>
    
    public class PhotosGetCountTests : BaseTest
    {
        [Fact]
        [Trait("Category","AccessTokenRequired")]

        public void PhotosGetCountTakenTest()
        {
            Flickr f = AuthInstance;

            var dates = new List<DateTime>();
            var date1 = new DateTime(2009, 1, 12);
            var date2 = new DateTime(2009, 9, 12);
            var date3 = new DateTime(2009, 12, 12);

            dates.Add(date2);
            dates.Add(date1);
            dates.Add(date3);

            PhotoCountCollection counts = f.PhotosGetCounts(dates.ToArray(), true);

            Assert.NotNull(counts);
            Assert.Equal(2, counts.Count);//, "PhotoCounts.Count should be two."

            Console.WriteLine(f.LastResponse);

            Assert.Equal(date1, counts[0].FromDate);//, "FromDate should be 12th January."
            Assert.Equal(date2, counts[0].ToDate);//, "ToDate should be 12th July."
            Assert.Equal(date2, counts[1].FromDate);//, "FromDate should be 12th July."
            Assert.Equal(date3, counts[1].ToDate);//, "ToDate should be 12th December."

        }

        [Fact]
        [Trait("Category","AccessTokenRequired")]

        public void PhotosGetCountUloadTest()
        {
            Flickr f = AuthInstance;

            var dates = new List<DateTime>();
            var date1 = new DateTime(2009, 7, 12);
            var date2 = new DateTime(2009, 9, 12);
            var date3 = new DateTime(2009, 12, 12);

            dates.Add(date2);
            dates.Add(date1);
            dates.Add(date3);

            PhotoCountCollection counts = f.PhotosGetCounts(dates.ToArray(), false);

            Assert.NotNull(counts);
            Assert.Equal(2, counts.Count);//, "PhotoCounts.Count should be two."

            Assert.Equal(date1, counts[0].FromDate);//, "FromDate should be 12th July."
            Assert.Equal(date2, counts[0].ToDate);//, "ToDate should be 12th September."
            Assert.Equal(date2, counts[1].FromDate);//, "FromDate should be 12th September."
            Assert.Equal(date3, counts[1].ToDate);//, "ToDate should be 12th December."

        }
    }
}
