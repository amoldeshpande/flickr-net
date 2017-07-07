using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading.Tasks;

namespace FlickrNet
{
    public partial class Flickr
    {
        /// <summary>
        /// Gets an array of supported method names for Flickr.
        /// </summary>
        /// <remarks>
        /// Note: Not all methods might be supported by the FlickrNet Library.</remarks>
       
        public async Task<FlickrResult<MethodCollection>> ReflectionGetMethodsAsync()
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.reflection.getMethods");

            return await GetResponseAsync<MethodCollection>(parameters);
        }

        /// <summary>
        /// Gets the method details for a given method.
        /// </summary>
        /// <param name="methodName">The name of the method to retrieve.</param>
       
        public async Task<FlickrResult<Method>> ReflectionGetMethodInfoAsync(string methodName)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.reflection.getMethodInfo");
            parameters.Add("api_key", apiKey);
            parameters.Add("method_name", methodName);

            return await GetResponseAsync<Method>(parameters);
        }

    }
}
