using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace FlickrNet
{
    public static partial class FlickrResponder
    {
        struct QueueItem
        {
            public HttpRequestMessage request;
            public Action<HttpResponseMessage> callback;
        }
        static BlockingCollection<QueueItem> taskQueue;
        static HttpClient httpClient;
        static Task backGroundTask; 
        static FlickrResponder()
        {
            httpClient = new HttpClient();
            taskQueue = new BlockingCollection<QueueItem>();
            backGroundTask = new Task(
                async () =>
                {
                    while(!taskQueue.IsCompleted)
                    {
                        try
                        {
                            var tsk = taskQueue.Take();
                            var response = await httpClient.SendAsync(tsk.request);
                            tsk.callback(response);
                        }
                        catch
                        {

                        }
                    }
                }
                );
            backGroundTask.Start();

        }
        static void CleanUp()
        {
            taskQueue.CompleteAdding();
            backGroundTask.Wait();
            httpClient.Dispose();
        }

        /// <summary>
        /// Gets a data response for the given base url and parameters, 
        /// either using OAuth or not depending on which parameters were passed in.
        /// </summary>
        /// <param name="flickr">The current instance of the <see cref="Flickr"/> class.</param>
        /// <param name="baseUrl">The base url to be called.</param>
        /// <param name="parameters">A dictionary of parameters.</param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static void GetDataResponseAsync(Flickr flickr, string baseUrl, Dictionary<string, string> parameters, Action<FlickrResult<string>> callback)
        {
            bool oAuth = parameters.ContainsKey("oauth_consumer_key");

            if (oAuth)
                GetDataResponseOAuthAsync(flickr, baseUrl, parameters, callback);
            else
                GetDataResponseNormalAsync(flickr, baseUrl, parameters, callback);
        }

        private static void GetDataResponseNormalAsync(Flickr flickr, string baseUrl, Dictionary<string, string> parameters, Action<FlickrResult<string>> callback) 
        {
            var method = flickr.CurrentService == SupportedService.Zooomr ? "GET" : "POST";

            var data = string.Empty;

            foreach (var k in parameters)
            {
                data += k.Key + "=" + UtilityMethods.EscapeDataString(k.Value) + "&";
            }

            if (method == "GET" && data.Length > 2000) method = "POST";

            if (method == "GET")
                DownloadDataAsync(method, baseUrl + "?" + data, null, null, null, callback);
            else
                DownloadDataAsync(method, baseUrl, data, PostContentType, null, callback);
        }

        private static void GetDataResponseOAuthAsync(Flickr flickr, string baseUrl, Dictionary<string, string> parameters, Action<FlickrResult<string>> callback)
        {
            const string method = "POST";

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
                DownloadDataAsync(method, baseUrl, data, PostContentType, authHeader, callback);
            }
            catch (HttpRequestException ex)
            {
                //var response = ex.Response as HttpWebResponse;
                //if (response == null) throw;

                //if (response.StatusCode != HttpStatusCode.BadRequest && response.StatusCode != HttpStatusCode.Unauthorized) throw;

                //using (var responseReader = new StreamReader(response.GetResponseStream()))
                //{
                //    string responseData = responseReader.ReadToEnd();
                //    responseReader.Close();

                    throw new OAuthException(ex);
                //}
            }
        }

        private static void DownloadDataAsync(string method, string baseUrl, string data, string contentType, string authHeader, Action<FlickrResult<string>> callback)
        {
            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod(method), baseUrl);

            if ((data!= null) && (data.Length > 0))
            {
                request.Content = new StringContent(data);
            }

            if (!string.IsNullOrEmpty(contentType)) request.Content.Headers.Add("Content-Type", contentType);
            if (!string.IsNullOrEmpty(authHeader)) request.Headers.Add("Authorization", authHeader);

            taskQueue.Add(new QueueItem()
            {
                request = request,
                callback = async (HttpResponseMessage resp) =>
                {
                    var result = new FlickrResult<string>();
                    try
                    {
                        resp.EnsureSuccessStatusCode();
                    }
                    catch (Exception e)
                    {
                        result.Error = e;
                        callback(result);
                        return;
                    }

                    result.Result = await resp.Content.ReadAsStringAsync();
                    callback(result);
                    return;
                }
            });
            
        }
    }
}
