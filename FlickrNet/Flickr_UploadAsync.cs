﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Collections;
using System.Xml.Serialization;
using System.Xml;
using System.Threading.Tasks;
using System.Net.Http;

namespace FlickrNet
{
    public partial class Flickr
    {
        /// <summary>
        /// UploadPicture method that does all the uploading work.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> object containing the pphoto to be uploaded.</param>
        /// <param name="fileName">The filename of the file to upload. Used as the title if title is null.</param>
        /// <param name="title">The title of the photo (optional).</param>
        /// <param name="description">The description of the photograph (optional).</param>
        /// <param name="tags">The tags for the photograph (optional).</param>
        /// <param name="isPublic">false for private, true for public.</param>
        /// <param name="isFamily">true if visible to family.</param>
        /// <param name="isFriend">true if visible to friends only.</param>
        /// <param name="contentType">The content type of the photo, i.e. Photo, Screenshot or Other.</param>
        /// <param name="safetyLevel">The safety level of the photo, i.e. Safe, Moderate or Restricted.</param>
        /// <param name="hiddenFromSearch">Is the photo hidden from public searches.</param>
        public async Task<FlickrResult<string>> UploadPictureAsync(Stream stream, string fileName, string title, string description, string tags,
                                       bool isPublic, bool isFamily, bool isFriend, ContentType contentType,
                                       SafetyLevel safetyLevel, HiddenFromSearch hiddenFromSearch)
        {
            CheckRequiresAuthentication();

            var uploadUri = new Uri(UploadUrl);

            var parameters = new Dictionary<string, string>();

            if (title != null && title.Length > 0)
            {
                parameters.Add("title", title);
            }
            if (description != null && description.Length > 0)
            {
                parameters.Add("description", description);
            }
            if (tags != null && tags.Length > 0)
            {
                parameters.Add("tags", tags);
            }

            parameters.Add("is_public", isPublic ? "1" : "0");
            parameters.Add("is_friend", isFriend ? "1" : "0");
            parameters.Add("is_family", isFamily ? "1" : "0");

            if (safetyLevel != SafetyLevel.None)
            {
                parameters.Add("safety_level", safetyLevel.ToString("D"));
            }
            if (contentType != ContentType.None)
            {
                parameters.Add("content_type", contentType.ToString("D"));
            }
            if (hiddenFromSearch != HiddenFromSearch.None)
            {
                parameters.Add("hidden", hiddenFromSearch.ToString("D"));
            }

            parameters.Add("api_key", apiKey);

            if (!string.IsNullOrEmpty(OAuthAccessToken))
            {
                parameters.Remove("api_key");
                OAuthGetBasicParameters(parameters);
                parameters.Add("oauth_token", OAuthAccessToken);
                string sig = OAuthCalculateSignature("POST", uploadUri.AbsoluteUri, parameters, OAuthAccessTokenSecret);
                parameters.Add("oauth_signature", sig);
            }
            else
            {
                parameters.Add("auth_token", apiToken);
            }

            return await UploadDataAsync(stream, fileName, uploadUri, parameters);
        }

        /// <summary>
        /// Replace an existing photo on Flickr.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> object containing the photo to be uploaded.</param>
        /// <param name="fileName">The filename of the file to replace the existing item with.</param>
        /// <param name="photoId">The ID of the photo to replace.</param>
       
        public async Task<FlickrResult<string>> ReplacePictureAsync(Stream stream, string fileName, string photoId)
        {
            var replaceUri = new Uri(ReplaceUrl);

            var parameters = new Dictionary<string, string>();

            parameters.Add("photo_id", photoId);
            parameters.Add("api_key", apiKey);
            parameters.Add("auth_token", apiToken);

            return await UploadDataAsync(stream, fileName, replaceUri, parameters);
        }

        private async Task<FlickrResult<string>> UploadDataAsync(Stream imageStream, string fileName, Uri uploadUri, Dictionary<string, string> parameters)
        {
            string boundary = "FLICKR_MIME_" + DateTime.Now.ToString("yyyyMMddhhmmss", System.Globalization.DateTimeFormatInfo.InvariantInfo);

            string authHeader = FlickrResponder.OAuthCalculateAuthHeader(parameters);

            var dataBuffer = CreateUploadData(imageStream, fileName, parameters, boundary);

            var req = new HttpRequestMessage(new HttpMethod("POST"), uploadUri);
            var ms = new MemoryStream();
            dataBuffer.CopyTo(ms);
            ms.Seek(0, SeekOrigin.Begin);
            req.Content = new StreamContent(ms);
            req.Content.Headers.Add("Content-Type", "multipart/form-data; boundary=" + boundary);
            if (!string.IsNullOrEmpty(authHeader))
            {
                req.Headers.Add("Authorization", authHeader);
            }

            var r2 = await httpClient.SendAsync(req);
            {
                var result = new FlickrResult<string>();

                try
                {
                    r2.EnsureSuccessStatusCode();
                    var responseXml = await r2.Content.ReadAsStringAsync();

                    var t = new UnknownResponse();
                    ((IFlickrParsable)t).Load(responseXml);
                    result.Result = t.GetElementValue("photoid");
                    result.HasError = false;
                }
                catch (Exception ex)
                {
                    result.Error = ex;
                }
                return (result);
            }

        }
    }
}
