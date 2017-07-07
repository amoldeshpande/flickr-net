using System;
using Xunit;
using FlickrNet;

namespace FlickrNetTest
{
    
    public class InterestingnessTests : BaseTest
    {
        [Fact]
        public void InterestingnessGetListTestsBasicTest()
        {
            DateTime date = DateTime.Today.AddDays(-2);
            DateTime datePlusOne = date.AddDays(1);

            PhotoCollection photos = Instance.InterestingnessGetList(date, PhotoSearchExtras.All, 1, 100);

            Assert.NotNull(photos);

            Assert.True(photos.Count > 50 && photos.Count <= 100, "Count should be at least 50, but not more than 100.");
        }
    }
}
