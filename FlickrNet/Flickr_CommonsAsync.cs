using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FlickrNet
{
    public partial class Flickr
    {
        /// <summary>
        /// Gets a collection of Flickr Commons institutions.
        /// </summary>
       
        public async Task <FlickrResult<InstitutionCollection>> CommonsGetInstitutionsAsync()
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.commons.getInstitutions");

            return await GetResponseAsync<InstitutionCollection>(parameters);
        }
    }
}
