using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Xml;
using System.Threading.Tasks;

namespace FlickrNet
{
    public partial class Flickr
    {
        /// <summary>
        /// Gets the currently authenticated users default content type.
        /// </summary>
       
        public async Task<FlickrResult<ContentType>> PrefsGetContentTypeAsync()
        {
            CheckRequiresAuthentication();

            var parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.prefs.getContentType");

            var r = await GetResponseAsync<UnknownResponse>(parameters);
            var result = new FlickrResult<ContentType>();
            result.Error = r.Error;
            if (!r.HasError)
            {
                result.Result = (ContentType)int.Parse(r.Result.GetAttributeValue("*", "content_type"), System.Globalization.NumberFormatInfo.InvariantInfo);
            }
            return (result);
        }

        /// <summary>
        /// Returns the default privacy level for geographic information attached to the user's photos and whether 
        /// or not the user has chosen to use geo-related EXIF information to automatically geotag their photos.
        /// </summary>
       
        public async Task<FlickrResult<UserGeoPermissions>> PrefsGetGeoPermsAsync()
        {
            CheckRequiresAuthentication();

            var parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.prefs.getGeoPerms");

            return await GetResponseAsync<UserGeoPermissions>(parameters);
        }

        /// <summary>
        /// Gets the currently authenticated users default hidden from search setting.
        /// </summary>
       
        public async  Task<FlickrResult<HiddenFromSearch>> PrefsGetHiddenAsync()
        {
            CheckRequiresAuthentication();

            var parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.prefs.getHidden");

            var r = await GetResponseAsync<UnknownResponse>(parameters);
            var result = new FlickrResult<HiddenFromSearch>();
            result.Error = r.Error;
            if (!r.HasError)
            {
                result.Result = (HiddenFromSearch)int.Parse(r.Result.GetAttributeValue("*", "hidden"), System.Globalization.NumberFormatInfo.InvariantInfo);
            }
            return (result);
        }

        /// <summary>
        /// Returns the default privacy level preference for the user. 
        /// </summary>
       
        public async Task<FlickrResult<PrivacyFilter>> PrefsGetPrivacyAsync()
        {
            CheckRequiresAuthentication();

            var parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.prefs.getPrivacy");

            var r = await GetResponseAsync<UnknownResponse>(parameters);
            var result = new FlickrResult<PrivacyFilter>();
            result.Error = r.Error;
            if (!r.HasError)
            {
                result.Result = (PrivacyFilter)int.Parse(r.Result.GetAttributeValue("*", "privacy"), System.Globalization.NumberFormatInfo.InvariantInfo);
            }
            return (result);

        }

        /// <summary>
        /// Gets the currently authenticated users default safety level.
        /// </summary>
       
        public async Task<FlickrResult<SafetyLevel>> PrefsGetSafetyLevelAsync()
        {
            CheckRequiresAuthentication();

            var parameters = new Dictionary<string, string>();
            parameters.Add("method", "flickr.prefs.getSafetyLevel");

            var r = await GetResponseAsync<UnknownResponse>(parameters);
            var result = new FlickrResult<SafetyLevel>();
            result.Error = r.Error;
            if (!r.HasError)
            {
                result.Result = (SafetyLevel)int.Parse(r.Result.GetAttributeValue("*", "safety_level"), System.Globalization.NumberFormatInfo.InvariantInfo);
            }
            return (result);
        }

    }
}
