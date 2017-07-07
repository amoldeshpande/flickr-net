using System;
using Xunit;
using FlickrNet;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for StatsGetTotalViewsTest
    /// </summary>
    [Trait("Category","AccessTokenRequired")]
    public class StatsGetTotalViewsTest : BaseTest
    {
        [Fact]
        public void StatsGetTotalViewsBasicTest()
        {
            StatViews views = AuthInstance.StatsGetTotalViews();

            Assert.NotNull(views);// "StatViews should not be null.");
            Assert.NotEqual(0, views.TotalViews);// "TotalViews should be greater than zero."
            Assert.NotEqual(0, views.PhotostreamViews);// "PhotostreamViews should be greater than zero."
            Assert.NotEqual(0, views.PhotoViews);// "PhotoViews should be greater than zero."
        }

        [Fact]
        public void StatGetCsvFilesTest()
        {
            CsvFileCollection col = AuthInstance.StatsGetCsvFiles();

            Assert.NotNull(col);// "CsvFileCollection should not be null.");

            Assert.True(col.Count > 1);// "Should be more than one CsvFile returned.");

            foreach (var file in col)
            {
                Assert.NotNull(file.Href);// "Href should not be null.");
                Assert.NotNull(file.Type);// "Type should not be null.");
                Assert.NotEqual(DateTime.MinValue, file.Date);
            }
        }
    }
}
