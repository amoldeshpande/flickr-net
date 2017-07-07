using System;
using System.Net;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FlickrNet
{
    public partial class Flickr
    {
        HttpClient httpClient = new HttpClient();
        private async Task GetResponseEvent<T>(Dictionary<string, string> parameters, EventHandler<FlickrResultArgs<T>> handler) where T : IFlickrParsable, new()
        {
            var r = await GetResponseAsync<T>(parameters);
            handler(this, new FlickrResultArgs<T>(r));
        }

        private async Task<FlickrResult<T>> GetResponseAsync<T>(Dictionary<string, string> parameters) where T : IFlickrParsable, new()
        {
            CheckApiKey();

            parameters["api_key"] = ApiKey;

            // If performing one of the old 'flickr.auth' methods then use old authentication details.
            string method = parameters["method"];
            
            if (method.StartsWith("flickr.auth", StringComparison.Ordinal) && !method.EndsWith("oauth.checkToken", StringComparison.Ordinal))
            {
                if (!string.IsNullOrEmpty(AuthToken)) parameters["auth_token"] = AuthToken;
            }
            else
            {
                // If OAuth Token exists or no authentication required then use new OAuth
                if (!string.IsNullOrEmpty(OAuthAccessToken) || string.IsNullOrEmpty(AuthToken))
                {
                    OAuthGetBasicParameters(parameters);
                    if (!string.IsNullOrEmpty(OAuthAccessToken)) parameters["oauth_token"] = OAuthAccessToken;
                }
                else
                {
                    parameters["auth_token"] = AuthToken;
                }
            }

            var url = CalculateUri(parameters, !string.IsNullOrEmpty(sharedSecret));

            lastRequest = url;
            var result = new FlickrResult<T>();

            try
            {
                var r = await FlickrResponder.GetDataResponseAsync(this, BaseUri.AbsoluteUri, parameters);
                if (!r.HasError)
                {
                    var t = new T();
                    ((IFlickrParsable)t).Load(r.Result);
                    result.Result = t;
                    result.HasError = false;
                }
                else
                {
                    result.Error = r.Error;
                }
            }
            catch (Exception ex)
            {
                result = new FlickrResult<T>();
                result.Error = ex;
            }

            return result;
        }

        private async Task<FlickrResult<T>> DoGetResponseAsync<T>(Uri url) where T : IFlickrParsable, new()
        {
            string postContents = string.Empty;
            var result = new FlickrResult<T>();

            if (url.AbsoluteUri.Length > 2000)
            {
                postContents = url.Query.Substring(1);
                url = new Uri(url, string.Empty);
            }


            var request = new HttpRequestMessage(new HttpMethod("POST"), url);
            request.Content = new StringContent(postContents);
            request.Content.Headers.Add("ContentType" , "application/x-www-form-urlencoded");

            var response = await httpClient.SendAsync(request);
            try
            {
                response.EnsureSuccessStatusCode();
                String responseXml = await response.Content.ReadAsStringAsync();
                var t = new T();
                ((IFlickrParsable)t).Load(responseXml);
                result.Result = t;
                result.HasError = false;
            }
            catch(HttpRequestException ex)
            {
                result.Error = ex;

            }
            return result; 
        }
    }
}
