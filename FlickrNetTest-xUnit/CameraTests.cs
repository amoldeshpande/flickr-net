using System.Linq;
using Xunit;

namespace FlickrNetTest
{
    
    public class CameraTests : BaseTest
    {
        [Fact]
        public void ShouldReturnListOfCameraBrands()
        {
            var brands = Instance.CamerasGetBrands();

            Assert.NotNull((brands));
            Assert.NotEqual(0, brands.Count);

            Assert.True(brands.Any(b => b.BrandId == "canon" && b.BrandName == "Canon"));
            Assert.True(brands.Any(b => b.BrandId == "nikon" && b.BrandName == "Nikon"));
        }

        [Fact]
        public void ShouldReturnListOfCanonCameraModels()
        {
            var models = Instance.CamerasGetBrandModels("canon");

            Assert.NotNull((models));
            Assert.NotEqual(0, models.Count);

            Assert.True(models.Any(c => c.CameraId == "eos_5d_mark_ii" && c.CameraName == "Canon EOS 5D Mark II"));
            Assert.True(models.Any(c => c.CameraId == "powershot_a620" && c.CameraName == "Canon PowerShot A620"));
            
        }
    }
}
