using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading.Tasks;

#pragma warning disable CS0618 // Type or member is obsolete

namespace FlickrNet
{
    public partial class Flickr
    {
        /// <summary>
        /// Checks the currently set authentication token with the flickr service to make
        /// sure it is still valid.
        /// </summary>
        public async Task<FlickrResult<Auth>> AuthCheckTokenAsync()
        {
            return await AuthCheckTokenAsync(AuthToken);
        }

        /// <summary>
        /// Checks a authentication token with the flickr service to make
        /// sure it is still valid.
        /// </summary>
        /// <param name="token">The authentication token to check.</param>
        public async Task<FlickrResult<Auth>> AuthCheckTokenAsync(string token)
        {
            CheckSigned();

            var parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.auth.checkToken");
            parameters.Add("auth_token", token);

            var r = await GetResponseAsync<Auth>(parameters);
            if (!r.HasError)
            {
                AuthToken = r.Result.Token;
            }
            return (r);
        }

        /// <summary>
        /// Takes the currently (old) authentication Flickr instance and turns it OAuth authenticated instance.
        /// </summary>
        /// <remarks>
        /// Calling this method will also clear <see cref="Flickr.AuthToken"/> 
        /// and set <see cref="Flickr.OAuthAccessToken"/> and <see cref="Flickr.OAuthAccessTokenSecret"/>.
        /// </remarks>
       
        public async Task<FlickrResult<OAuthAccessToken>> AuthOAuthGetAccessTokenAsync()
        {
            CheckRequiresAuthentication();

            var parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.auth.oauth.getAccessToken");

            var r = await GetResponseAsync<OAuthAccessToken>(parameters);
            if (!r.HasError)
            {
                OAuthAccessToken = r.Result.Token;
                OAuthAccessTokenSecret = r.Result.TokenSecret;

                AuthToken = null;
            }

            return (r);
        }

        /// <summary>
        /// Checks the OAuth token, returns user information and permissions if valid.
        /// </summary>
        /// <returns></returns>
        public async Task<FlickrResult<Auth>> AuthOAuthCheckTokenAsync()
        {
            CheckRequiresAuthentication();

            var parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.auth.oauth.checkToken");

            return await GetResponseAsync<Auth>(parameters);
        }


    }
}
