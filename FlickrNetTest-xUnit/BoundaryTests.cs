using FlickrNet;
using Xunit;
using System;
using Shouldly;

namespace FlickrNetTest
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Potential Code Quality Issues", "RECS0026:Possible unassigned object created by 'new'")]
    public class BoundaryTests : BaseTest
    {
        [Fact]
        public void BoundaryBoxCalculateSizesUkNewcastle()
        {
            var b = BoundaryBox.UKNewcastle;

            var e = b.DiagonalDistanceInMiles();

            Assert.NotEqual(0, e);
        }

        [Theory]
        [InlineData(-180, -91, 180, 90)]
        [InlineData(-181, -90, 180, 90)]
        [InlineData(-180, -90, 180, 91)]
        [InlineData(-180, -90, 181, 90)]
        public void BoundaryBoxThrowExceptionOnInvalidMinLat(double minLong, double minLat, double maxLong, double maxLat)
        {
            Should.Throw<ArgumentOutOfRangeException>(() => new BoundaryBox(minLong, minLat, maxLong, maxLat));
        }

        [Fact]
        public void BoundaryBoxCalculateSizesFrankfurtToBerlin()
        {
            var b = new BoundaryBox(8.68194, 50.11222, 13.29750, 52.52222);

            var e = b.DiagonalDistanceInMiles();
            Assert.True(259.9 < e && e < 260.0);
        }

        [Fact]
        public void BoundaryBoxWithNullPointStringThrows()
        {
            Should.Throw<ArgumentNullException>(() => new BoundaryBox(null));
        }

        [Fact]
        public void BoundaryBoxWithInvalidPointStringThrows()
        {
            Should.Throw<ArgumentException>(() => new BoundaryBox("1,2,3"));
        }

        [Fact]
        public void BoundaryBoxWithNoneNumericPointStringThrows()
        {
            Should.Throw<ArgumentException>(() => new BoundaryBox("1,2,3,A"));
        }

        [Fact(Skip ="Doesn't compile with xunit")]
        public void BoundaryBoxWithValidPointStringSetCorrectly()
        {
            var b = new BoundaryBox("1,2,3,4");

            //Assert.Equal(b.MinimumLongitude, 1M);
            //Assert.Equal(b.MinimumLatitude, (2M));
            //Assert.Equal(b.MaximumLongitude, (3M));
            //Assert.Equal(b.MaximumLatitude, (4M));
        }

    }
}
