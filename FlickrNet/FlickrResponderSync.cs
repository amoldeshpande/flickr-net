using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Threading;

namespace FlickrNet
{
    public static partial class FlickrResponder
    {
        /// <summary>
        /// Gets a data response for the given base url and parameters, 
        /// either using OAuth or not depending on which parameters were passed in.
        /// </summary>
        /// <param name="flickr">The current instance of the <see cref="Flickr"/> class.</param>
        /// <param name="baseUrl">The base url to be called.</param>
        /// <param name="parameters">A dictionary of parameters.</param>
        /// <returns></returns>
        public static string GetDataResponse(Flickr flickr, string baseUrl, Dictionary<string, string> parameters)
        {
            bool oAuth = parameters.ContainsKey("oauth_consumer_key");

            if (oAuth)
                return GetDataResponseOAuth(flickr, baseUrl, parameters);
            else
                return GetDataResponseNormal(flickr, baseUrl, parameters);
        }

        private static string GetDataResponseNormal(Flickr flickr, string baseUrl, Dictionary<string, string> parameters)
        {
            string method = flickr.CurrentService == SupportedService.Zooomr ? "GET" : "POST";

            string data = string.Empty;

            foreach (var k in parameters)
            {
                data += k.Key + "=" + UtilityMethods.EscapeDataString(k.Value) + "&";
            }

            if (method == "GET" && data.Length > 2000) method = "POST";

            if (method == "GET")
                return DownloadData(method, baseUrl + "?" + data, null, null, null);
            else
                return DownloadData(method, baseUrl, data, PostContentType, null);
        }

        private static string GetDataResponseOAuth(Flickr flickr, string baseUrl, Dictionary<string, string> parameters)
        {
            string method = "POST";

            // Remove api key if it exists.
            if (parameters.ContainsKey("api_key")) parameters.Remove("api_key");
            if (parameters.ContainsKey("api_sig")) parameters.Remove("api_sig");

            // If OAuth Access Token is set then add token and generate signature.
            if (!string.IsNullOrEmpty(flickr.OAuthAccessToken) && !parameters.ContainsKey("oauth_token"))
            {
                parameters.Add("oauth_token", flickr.OAuthAccessToken);
            }
            if (!string.IsNullOrEmpty(flickr.OAuthAccessTokenSecret) && !parameters.ContainsKey("oauth_signature"))
            {
                string sig = flickr.OAuthCalculateSignature(method, baseUrl, parameters, flickr.OAuthAccessTokenSecret);
                parameters.Add("oauth_signature", sig);
            }

            // Calculate post data, content header and auth header
            string data = OAuthCalculatePostData(parameters);
            string authHeader = OAuthCalculateAuthHeader(parameters);

            // Download data.
            try
            {
                return DownloadData(method, baseUrl, data, PostContentType, authHeader);
            }
            catch (HttpRequestException ex)
            {
                //if (ex.Status != WebExceptionStatus.ProtocolError) throw;

                //var response = ex.Response as HttpWebResponse;
                //if (response == null) throw;

                //string responseData = null;

                //using (var stream = response.GetResponseStream())
                //{
                //    if( stream != null)
                //        using (var responseReader = new StreamReader(stream))
                //        {
                //            responseData = responseReader.ReadToEnd();
                //            responseReader.Close();
                //        }
                //}
                //if (response.StatusCode == HttpStatusCode.BadRequest ||
                //    response.StatusCode == HttpStatusCode.Unauthorized)
                //{

                    throw new OAuthException( ex);
                //}

                //if (string.IsNullOrEmpty(responseData)) throw;
                //throw new WebException("WebException occurred with the following body content: " + responseData, ex, ex.Status, ex.Response);
            }
        }


        private static string DownloadData(string method, string baseUrl, string data, string contentType, string authHeader)
        {
            var task = DownloadDataAsync(method, baseUrl, data, contentType, authHeader);
            task.Wait();
            var result = task.Result;
            return  result.Result;
        }

    }
}
