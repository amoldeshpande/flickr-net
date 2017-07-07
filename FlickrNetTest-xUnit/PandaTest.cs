
using Xunit;
using FlickrNet;

namespace FlickrNetTest
{
    /// <summary>
    /// Summary description for PandaGetListTest
    /// </summary>
    
    public class PandaTest : BaseTest
    {
        [Fact]
        public void PandaGetListBasicTest()
        {
            string[] pandas = Instance.PandaGetList();

            Assert.NotNull(pandas);
            Assert.True(pandas.Length > 0, "Should not return empty array");

            Assert.Equal("ling ling", pandas[0]);
            Assert.Equal("hsing hsing", pandas[1]);
            Assert.Equal("wang wang", pandas[2]);
        }

        [Fact]
        public void PandaGetPhotosLingLingTest()
        {
            var photos = Instance.PandaGetPhotos("ling ling");

            Assert.NotNull(photos);
            Assert.Equal(photos.Count, photos.Total);//, "PandaPhotos.Count should equal PandaPhotos.Total."
            Assert.Equal("ling ling", photos.PandaName);//, "PandaPhotos.Panda should be 'ling ling'"
        }
    }
}
