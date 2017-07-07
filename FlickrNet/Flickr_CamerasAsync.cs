using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FlickrNet
{
    public partial class Flickr
    {
        /// <summary>
        /// Gets a list of camera brands.
        /// </summary>
       
        /// <returns></returns>
        public async Task<FlickrResult<BrandCollection>> CamerasGetBrandsAsync( )
        {
            var parameters = new Dictionary<string, string> { { "method", "flickr.cameras.getBrands" } };
            return await GetResponseAsync<BrandCollection>(parameters);
        }

        /// <summary>
        /// Get a list of camera models for a particular brand id.
        /// </summary>
        /// <param name="brandId">The ID of the brand you want the models of.</param>
       
        /// <returns></returns>
        public async Task<FlickrResult<CameraCollection>> CamerasGetBrandModelsAsync(string brandId)
        {
            var parameters = new Dictionary<string, string>
                                 {
                                     {"method", "flickr.cameras.getBrandModels"},
                                     {"brand", brandId}
                                 };
            return await GetResponseAsync<CameraCollection>(parameters);
        }
    }
}
